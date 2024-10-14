using MediatR;
using MongoDB.Bson;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Places.Create;

public class RequestHandler : IRequestHandler<Request, Place>
{
    private readonly IMongoContext _mongoContext;

    public RequestHandler(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }

    public async Task<Place> Handle(Request request, CancellationToken cancellationToken)
    {
        var location = request.Body.Location;
        location.Name = request.Body.Name;
        
        await _mongoContext.Locations.InsertOneAsync(location, cancellationToken: cancellationToken);
        
        var place = new Place()
        {
            Id = ObjectId.GenerateNewId(),
            PlaceType = request.Body.Type,
            CreatorId = request.Body.CreatorId,
            Name = request.Body.Name,
            Description = request.Body.Description,
            Url = request.Body.Url,
            Mail = request.Body.Mail,
            Polygon = request.Body.Polygon,
            Capacity = request.Body.Capacity,
            ImageUrls = request.Body.ImageUrls
        };
        
        await _mongoContext.Places.InsertOneAsync(place, cancellationToken: cancellationToken);

        return place;
    }
}