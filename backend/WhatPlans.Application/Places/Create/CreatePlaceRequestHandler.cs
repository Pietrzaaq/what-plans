using MediatR;
using MongoDB.Bson;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Places.Create;

public class CreatePlaceRequestHandler : IRequestHandler<CreatePlaceRequest, Place>
{
    private readonly IMongoContext _mongoContext;

    public CreatePlaceRequestHandler(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }

    public async Task<Place> Handle(CreatePlaceRequest request, CancellationToken cancellationToken)
    {
        var place = new Place()
        {
            Id = ObjectId.GenerateNewId(),
            Name = request.Place.Name,
        };
        
        await _mongoContext.Places.InsertOneAsync(place, cancellationToken: cancellationToken);

        return place;
    }
}