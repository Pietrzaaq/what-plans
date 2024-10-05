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
            PlaceId = request.Body.PlaceId,
            LocationId = request.Body.LocationId,
            CreatorId = request.Body.CreatorId,
            EventType = request.Body.Type,
            Name = request.Body.Name,
            StartDate = request.Body.StartDate,
            EndDate = request.Body.EndDate,
            Duration = request.Body.EndDate is null ? null : (request.Body.EndDate - request.Body.StartDate).Value.Minutes,
            ImageUrls = request.Body.ImageUrls
        };
        
        await _mongoContext.Events.InsertOneAsync(newEvent, cancellationToken: cancellationToken);

        return newEvent;
    }
}