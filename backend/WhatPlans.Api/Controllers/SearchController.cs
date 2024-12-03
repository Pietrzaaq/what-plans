using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Api.Controllers;

[Route("api/search")]
public class SearchController : BaseController
{
    private readonly IMongoContext _mongoContext;

    public SearchController(IMediator mediator, IMongoContext mongoContext)
        : base(mediator)
    {
        _mongoContext = mongoContext;
    }
    
    [HttpGet]
    public async Task<IActionResult> Autocomplete([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
        {
            return BadRequest("Query must be at least 2 characters.");
        }
        
        var eventFilter = Builders<Event>.Filter.Regex(e => e.Name, new BsonRegularExpression(query, "i"));
        var events = await _mongoContext.Events
            .Find(eventFilter)
            .SortBy(e => e.StartDate)
            .ToListAsync();
        
        var placeFilter = Builders<Place>.Filter.Regex(p => p.Name, new BsonRegularExpression(query, "i"));
        var places = await _mongoContext.Places
            .Find(placeFilter)
            .ToListAsync();

        var results = new List<SearchDto>();

        foreach (var @event in events)
        {
            var place = await _mongoContext.Places
                .Find(p => p.Id == @event.PlaceId)
                .FirstOrDefaultAsync();
            
            var searchDto = new SearchDto()
            {
                Id = @event.Id,
                Name = @event.Name,
                Type = SearchDtoType.Event,
                ImageUrls = @event.ImageUrls,
                CreatorId = @event.CreatorId,
                StartDate = @event.StartDate,
                Latitude = place?.Location.Latitude,
                Longitude = place?.Location.Longitude,
                CityName = place?.Location?.CityName,
                CityId = place?.Location?.CityId,
                Address = place?.Location?.Address
            };
            
            results.Add(searchDto);
        }
        
        foreach (var place in places)
        {
            var searchDto = new SearchDto()
            {
                Id = place.Id,
                Name = place.Name,
                Type = SearchDtoType.Place,
                ImageUrls = place.ImageUrls,
                CreatorId = place.CreatorId,
                StartDate = null,
                Latitude = place.Location.Latitude,
                Longitude = place.Location.Longitude,
                CityName = place.Location.CityName,
                CityId = place.Location.CityId,
                Address = place.Location.Address,
            };
            
            results.Add(searchDto);
        }
        
        return Ok(results);
    }

    public class SearchDto
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public SearchDtoType Type { get; set; }
        public List<string> ImageUrls { get; set; }
        public string CreatorId { get; set; }
        public DateTime? StartDate { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string CityName { get; set; }
        public string CityId { get; set; }
        public string Address { get; set; }
    }

    public enum SearchDtoType
    {
        Event, 
        Place
    }
}
