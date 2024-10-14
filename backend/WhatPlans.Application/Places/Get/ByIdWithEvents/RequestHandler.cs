using MediatR;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Places.Get.ByIdWithEvents;

public class RequestHandler : IRequestHandler<Request, PlaceWithEvents>
{
    private readonly IMongoContext _mongoContext;

    public RequestHandler(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }
    
    public async Task<PlaceWithEvents> Handle(Request request, CancellationToken cancellationToken)
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
        
        return place;
    }
}