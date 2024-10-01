using MediatR;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Places.Get;

public class GetPlacesWithEventsRequestHandler : IRequestHandler<GetPlacesWithEventsRequest, List<PlaceWithEvents>>
{
    private readonly IMongoContext _mongoContext;

    public GetPlacesWithEventsRequestHandler(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }
    
    public async Task<List<PlaceWithEvents>> Handle(GetPlacesWithEventsRequest request, CancellationToken cancellationToken)
    {
        var queryableCollection = _mongoContext.Places.AsQueryable();
        
        var result = await _mongoContext.Places.Aggregate()
            .Lookup<Place, Event, PlaceWithEvents>(
                _mongoContext.Events,            
                place => place.Id,              
                eventItem => eventItem.PlaceId,  
                placeWithEvents => placeWithEvents.Events 
            )
            .ToListAsync(cancellationToken: cancellationToken);
        
        return result;
    }
}