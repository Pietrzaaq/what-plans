using MediatR;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Places.Get.ByIdWithEvents;

public class GetPlaceWithEventsRequestHandler : IRequestHandler<GetPlaceWithEventsRequest, PlaceWithEvents>
{
    private readonly IMongoContext _mongoContext;

    public GetPlaceWithEventsRequestHandler(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }
    
    public async Task<PlaceWithEvents> Handle(GetPlaceWithEventsRequest request, CancellationToken cancellationToken)
    {
        var filter = Builders<Place>.Filter.Eq(p => p.Id, request.Id);
        
        var place = await _mongoContext.Places.Aggregate()
            .Match(filter)
            .Lookup<Place, Event, PlaceWithEvents>(
                _mongoContext.Events,            
                place => place.Id,              
                eventItem => eventItem.PlaceId,  
                placeWithEvents => placeWithEvents.Events 
            )
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        
        var location = await _mongoContext.Locations.Find(l => l.Id == place.LocationId).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        place.Location = location;
        
        return place;
    }
}