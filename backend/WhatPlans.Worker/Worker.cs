using System.Globalization;
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
    private static readonly Random _random = new Random();

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
            var response = await client.GetAsync($"{_ticketMasterSettings.BaseUrl}/events?countryCode=PL&page={page}&apikey={_ticketMasterSettings.ApiKey}&size=100");

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

            int delay = _random.Next(5000, 7000);
            await Task.Delay(delay);
            page++;
        }
    }

    private async Task ProcessEvent(TicketMasterEvent ticketMasterEvent)
    {
        var venue = ticketMasterEvent.Embedded.Venues.FirstOrDefault();

        if (venue == null)
            return;
        
        var geohasher = new Geohasher();
        var geohash = geohasher.Encode(venue.Location.Latitude, venue.Location.Longitude, 8);
        var geohashes = geohasher.GetNeighbors(geohash).Select(geohashKeyValue => geohashKeyValue.Value).ToList();
        geohashes.Add(geohash);
        
        var nearbyPlace = await _mongoContext.Places
            .Find(p =>  geohashes.Any(g => p.Location.Geohash.Contains(g)) || p.Location.Name.Equals(venue.Name))
            .FirstOrDefaultAsync();

        var localDate = ticketMasterEvent.Dates.Start.LocalDate;
        var localTime = ticketMasterEvent.Dates.Start.LocalTime;

        if (localTime == null)
        {
            localTime = "00:00:00";
        }
        
        var dateTimeString = $"{localDate} {localTime}";
        
        DateTime localDateTime = DateTime.ParseExact(dateTimeString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

        var existingEvent = await _mongoContext.Events
            .Find(e => e.Name == ticketMasterEvent.Name && e.StartDate.Date == localDateTime.Date)
            .FirstOrDefaultAsync();

        if (existingEvent != null) 
            return;
        
        ObjectId? placeId;
        if (nearbyPlace == null)
        {
            var cityGeohash = new string(geohash.Take(4).ToArray());
            var cities = _mongoContext.Cities.Find(p => p.Geohash.Contains(cityGeohash)).ToList();

            if (cities == null || cities.Count == 0)
                return;

            var city = cities.OrderByDescending(c => c.Population).FirstOrDefault();

            if (city == null)
                return;
            
            var newPlace = new Place
            {
                Id = ObjectId.GenerateNewId(),
                Name = venue.Name,
                Location = new Domain.Entities.Location
                {
                    Latitude = venue.Location.Latitude,
                    Longitude = venue.Location.Longitude,
                    Address = venue.Address.Line1,
                    CityName = city.Name,
                    CityId = city.Id.ToString(),
                    FormatedAddress = $"{venue.Address.Line1}, {city.Name}",
                    Geohash = geohash
                },
                PlaceType = PlaceTypes.Other,
                Description = null,
                Url = venue.Url,
                GoogleMapsUrl = null,
                OpenStreetMapId = null,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                CreatorId = "Ticketmaster"
            };
    
            _logger.LogInformation($"Saving place from TicketMaster. Place: {newPlace.Name}, City: {city.Name}");

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
            PlaceId = placeId,
            CreatorId = "Ticketmaster"
        };

        _logger.LogInformation($"Saving event from TicketMaster. Event: {newEvent.Name}, Date: {newEvent.StartDate.ToShortDateString()}");
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
            "Arts & Theatre" => EventTypes.Cultural,
            "Sports" => EventTypes.Sport,
            "Food & Drink" => EventTypes.Culinary,
            _ => EventTypes.Other
        };
    }
}
