using MediatR;
using MongoDB.Bson;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Events.Create;

public class CreateEventRequestHandler : IRequestHandler<CreateEventRequest, Event>
{
    private readonly IMongoContext _mongoContext;

    public CreateEventRequestHandler(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }

    public async Task<Event> Handle(CreateEventRequest request, CancellationToken cancellationToken)
    {
        var newEvent = new Event()
        {
            Id = ObjectId.GenerateNewId(),
            Name = request.Event.Name,
        };
        
        await _mongoContext.Events.InsertOneAsync(newEvent, cancellationToken: cancellationToken);

        return newEvent;
    }
}