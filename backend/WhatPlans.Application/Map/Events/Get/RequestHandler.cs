using MediatR;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Map.Events.Get;

public class RequestHandler : IRequestHandler<Request, List<PlaceWithEvents>>
{
    private readonly IMongoContext _mongoContext;

    public RequestHandler(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }
    
    public async Task<List<PlaceWithEvents>> Handle(Request request, CancellationToken cancellationToken)
    {
        var geohashes = request.Geohashes;
        var startDate = request.StartDate;
        var endDate = request.EndDate;
        
        var placesFilter = Builders<Place>.Filter
            .Where(place => geohashes.Any(g => place.Location.Geohash.Contains(g)));

        var places = await _mongoContext.Places
            .Find(placesFilter)
            .ToListAsync(cancellationToken: cancellationToken);

        if (!places.Any())
            return new List<PlaceWithEvents>();
        
        var placeIds = places.Select(p => p.Id).ToList();

        FilterDefinition<Event> eventsFilter;
        if (startDate.HasValue && endDate.HasValue)
        {
            eventsFilter = Builders<Event>.Filter
                .Where(e => e.PlaceId.HasValue && 
                            placeIds.Contains(e.PlaceId.Value) && 
                            e.StartDate >= startDate && 
                            e.StartDate <= endDate);
        }
        else if (startDate.HasValue)
        {
            eventsFilter = Builders<Event>.Filter
                .Where(e => e.PlaceId.HasValue && 
                            placeIds.Contains(e.PlaceId.Value) && 
                            e.StartDate >= startDate);
        }
        else if (endDate.HasValue)
        {
            eventsFilter = Builders<Event>.Filter
                .Where(e => e.PlaceId.HasValue && 
                            placeIds.Contains(e.PlaceId.Value) && 
                            e.StartDate <= endDate);
        }
        else
        {
            eventsFilter = Builders<Event>.Filter
                .Where(e => e.PlaceId.HasValue && placeIds.Contains(e.PlaceId.Value));
        }

        var events = await _mongoContext.Events
            .Find(eventsFilter)
            .ToListAsync(cancellationToken: cancellationToken);

        var placeWithEventsList = places
            .Select(place => new PlaceWithEvents
            {
                Id = place.Id,
                PlaceType = place.PlaceType,
                Location = place.Location,
                CreatorId = place.CreatorId,
                Name = place.Name,
                Description = place.Description,
                Url = place.Url,
                Mail = place.Mail,
                Polygon = place.Polygon,
                Capacity = place.Capacity,
                Events = events.Where(e => e.PlaceId == place.Id).ToList()
            })
            .Where(p => p.Events.Any())
            .ToList();

        return placeWithEventsList;
    }
}