using MediatR;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;

using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Events.Delete;

public class DeleteEventRequestHandler : IRequestHandler<DeleteEventRequest>
{
    private readonly IMongoContext _mongoContext;

    public DeleteEventRequestHandler(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }
    
    public async Task Handle(DeleteEventRequest request, CancellationToken cancellationToken)
    {
        var filter = Builders<Event>.Filter.Eq(p => p.Id, request.Id);
        
        await _mongoContext.Events.DeleteOneAsync(filter, cancellationToken);
    }
}