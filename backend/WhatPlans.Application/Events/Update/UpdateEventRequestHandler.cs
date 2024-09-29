using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Events.Update;

public class UpdateEventRequestHandler : IRequestHandler<UpdateEventRequest, Event>
{
    private readonly IMongoContext _mongoContext;

    public UpdateEventRequestHandler(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }
    
    public async Task<Event> Handle(UpdateEventRequest request, CancellationToken cancellationToken)
    {
        var updatedEvent = new Event()
        {
            Id = new ObjectId(request.Id),
            Name = request.Body.Name
        };
        
        var filter = Builders<Event>.Filter.Eq(p => p.Id, updatedEvent.Id);
        
        var update = Builders<Event>.Update
            .Set(p => p.Name, updatedEvent.Name);
            
        await _mongoContext.Events.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        
        return updatedEvent;
    }
}