using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;

using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Places.Delete;

public class RequestHandler : IRequestHandler<Request>
{
    private readonly IMongoContext _mongoContext;

    public RequestHandler(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }
    
    public async Task Handle(Request request, CancellationToken cancellationToken)
    {
        var filter = Builders<Place>.Filter.Eq(p => p.Id, request.Id);
        
        await _mongoContext.Places.DeleteOneAsync(filter, cancellationToken);
    }
}