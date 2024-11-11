using Geohash;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;
using WhatPlans.Domain.Enums;
using WhatPlans.Worker.Models;

namespace WhatPlans.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IMongoContext _mongoContext;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly TicketMasterSettings _ticketMasterSettings;

    public Worker(
        ILogger<Worker> logger,
        IMongoContext mongoContext,
        IHttpClientFactory httpClientFactory,
        IOptions<TicketMasterSettings> ticketMasterSettings)
    {
        _logger = logger;
        _mongoContext = mongoContext;
        _httpClientFactory = httpClientFactory;
        _ticketMasterSettings = ticketMasterSettings.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await FetchAndStoreEventsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching events from TicketMaster.");
            }
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }

    private async Task FetchAndStoreEventsAsync()
    {
        var client = _httpClientFactory.CreateClient();
        int page = 0;
        bool moreResults = true;

        while (moreResults)
        {
            _logger.LogInformation($"Fetching events from TicketMaster. Page {page}");
            var response = await client.GetAsync($"{_ticketMasterSettings.BaseUrl}/events?countryCode=PL&page={page}&apikey={_ticketMasterSettings.ApiKey}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("TicketMaster API request failed.");
                return;
            }

            var content = await response.Content.ReadAsStringAsync();
            var apiResponseModel = JsonConvert.DeserializeObject<ApiResponse>(content);
            var events = apiResponseModel.Embedded.Events;
        
            if (events == null || events.Count == 0)
            {
                moreResults = false;
                continue;
            }

            foreach (var ticketMasterEvent in events)
            {
                await ProcessEvent(ticketMasterEvent);
            }

            page++;
        }
    }

    private async Task ProcessEvent(TicketMasterEvent ticketMasterEvent)
    {
        var venue = ticketMasterEvent.Embedded.Venues.FirstOrDefault();

        if (venue == null)
            return;
        
        var geohasher = new Geohasher();
        var geohash = geohasher.Encode(venue.Location.Latitude, venue.Location.Longitude, 7);
            
        var nearbyPlace = await _mongoContext.Places
            .Find(p => p.Location.Geohash.Contains(geohash))
            .FirstOrDefaultAsync();

        var localDateTime = DateTime.Parse(ticketMasterEvent.Dates.Start.LocalDate); 
        
        var existingEvent = await _mongoContext.Events
            .Find(e => e.Name == ticketMasterEvent.Name && e.StartDate == localDateTime)
            .FirstOrDefaultAsync();

        if (existingEvent != null) 
            return;
        
        ObjectId? placeId;
        
        if (nearbyPlace == null)
        {
            var newPlace = new Place
            {
                Id = ObjectId.GenerateNewId(),
                Name = venue.Name,
                Location = new WhatPlans.Domain.Entities.Location
                {
                    Latitude = venue.Location.Latitude,
                    Longitude = venue.Location.Longitude,
                    Address = venue.Address.Line1,
                    CityName = venue.City.Name,
                    FormatedAddress = $"{venue.Address.Line1}, venue.City.Name",
                    Geohash = geohasher.Encode(venue.Location.Latitude, venue.Location.Longitude)
                },
                PlaceType = PlaceTypes.Other,
                Description = null,
                Url = venue.Url,
                GoogleMapsUrl = null,
                OpenStreetMapId = null,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow
            };
    
            await _mongoContext.Places.InsertOneAsync(newPlace);
            placeId = newPlace.Id;
        }
        else
        {
            placeId = nearbyPlace.Id;
        }

        var images = ticketMasterEvent.Images.Select(i => i.Url).ToList();
        var newEvent = new Event
        {
            Id = ObjectId.GenerateNewId(),
            Name = ticketMasterEvent.Name,
            Url = ticketMasterEvent.Url,
            StartDate = localDateTime,
            EndDate = null,
            ImageUrls = images,
            EventType = MapToEventType(ticketMasterEvent.Classifications),
            PlaceId = placeId
        };

        await _mongoContext.Events.InsertOneAsync(newEvent);
    }

    private EventTypes MapToEventType(List<Classification> classifications)
    {
        var category = classifications.FirstOrDefault()?.Segment.Name;

        if (category is null)
            return EventTypes.Musical;
        
        return category switch
        {
            "Music" => EventTypes.Musical,
            "Sports" => EventTypes.Sport,
            "Food & Drink" => EventTypes.Culinary,
            _ => EventTypes.Other
        };
    }
}
