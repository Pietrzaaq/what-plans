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
        
        var placesFilter = Builders<Place>.Filter
            .Where(p => geohashes.Any(g => p.Location.Geohash.Contains(g)));
        
        var eventsFilter = Builders<PlaceWithEvents>.Filter.SizeGt(p => p.Events, 0);
        
        var placesWithEvents = await _mongoContext.Places.Aggregate()
            .Match(placesFilter)
            .Lookup<Place, Event, PlaceWithEvents>(
                _mongoContext.Events,            
                place => place.Id,              
                eventItem => eventItem.PlaceId,  
                placeWithEvents => placeWithEvents.Events 
            )
            .Match(eventsFilter)
            .ToListAsync(cancellationToken: cancellationToken);
        
        return placesWithEvents;
    }
}