using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;

using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Places.Delete;

public class DeletePlaceRequestHandler : IRequestHandler<DeletePlaceRequest>
{
    private readonly IMongoContext _mongoContext;

    public DeletePlaceRequestHandler(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }
    
    public async Task Handle(DeletePlaceRequest request, CancellationToken cancellationToken)
    {
        var filter = Builders<Place>.Filter.Eq(p => p.Id, new ObjectId(request.Id));
        
        await _mongoContext.Places.DeleteOneAsync(filter, cancellationToken);
    }
}