using MediatR;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;

using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Events.Delete;

public class RequestHandler : IRequestHandler<Request>
{
    private readonly IMongoContext _mongoContext;

    public RequestHandler(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }
    
    public async Task Handle(Request request, CancellationToken cancellationToken)
    {
        var filter = Builders<Event>.Filter.Eq(p => p.Id, request.Id);
        
        await _mongoContext.Events.DeleteOneAsync(filter, cancellationToken);
    }
}