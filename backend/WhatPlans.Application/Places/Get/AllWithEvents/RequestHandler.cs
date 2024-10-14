using MediatR;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Places.Get.AllWithEvents;

public class RequestHandler : IRequestHandler<Request, List<PlaceWithEvents>>
{
    private readonly IMongoContext _mongoContext;

    public RequestHandler(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }
    
    public async Task<List<PlaceWithEvents>> Handle(Request request, CancellationToken cancellationToken)
    {
        var filter = Builders<PlaceWithEvents>.Filter.SizeGt(p => p.Events, 0);
        
        var place = await _mongoContext.Places.Aggregate()
            .Lookup<Place, Event, PlaceWithEvents>(
                _mongoContext.Events,            
                place => place.Id,              
                eventItem => eventItem.PlaceId,  
                placeWithEvents => placeWithEvents.Events 
            )
            .Match(filter)
            .ToListAsync(cancellationToken: cancellationToken);
        
        return place;
    }
}