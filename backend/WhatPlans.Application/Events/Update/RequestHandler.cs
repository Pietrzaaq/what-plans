using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Events.Update;

public class RequestHandler : IRequestHandler<Request, Event>
{
    private readonly IMongoContext _mongoContext;

    public RequestHandler(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }
    
    public async Task<Event> Handle(Request request, CancellationToken cancellationToken)
    {
        var updatedEvent = new Event()
        {
            Id = request.Id,
            Name = request.Body.Name
        };
        
        var filter = Builders<Event>.Filter.Eq(p => p.Id, updatedEvent.Id);
        
        var update = Builders<Event>.Update
            .Set(p => p.Name, updatedEvent.Name);
            
        await _mongoContext.Events.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        
        return updatedEvent;
    }
}