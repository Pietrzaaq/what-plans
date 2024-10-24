﻿using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Places.Update;

public class RequestHandler : IRequestHandler<Request, Place>
{
    private readonly IMongoContext _mongoContext;

    public RequestHandler(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }
    
    public async Task<Place> Handle(Request request, CancellationToken cancellationToken)
    {
        var updatedPlace = new Place()
        {
            Id = request.Id,
            Name = request.Body.Name
        };
        
        var filter = Builders<Place>.Filter.Eq(p => p.Id, updatedPlace.Id);
        
        var update = Builders<Place>.Update
            .Set(p => p.Name, updatedPlace.Name);
            
        await _mongoContext.Places.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        
        return updatedPlace;
    }
}