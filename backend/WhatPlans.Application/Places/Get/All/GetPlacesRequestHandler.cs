using MediatR;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Places.Get.All;

public class GetPlacesRequestHandler : IRequestHandler<GetPlacesRequest, List<PlaceWithLocation>>
{
    private readonly IMongoContext _mongoContext;

    public GetPlacesRequestHandler(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }
    
    public async Task<List<PlaceWithLocation>> Handle(GetPlacesRequest request, CancellationToken cancellationToken)
    {
        var pipeline = new BsonDocument[]
        {
            new()
            {
                { "$lookup", new BsonDocument
                    {
                        { "from", "Locations" },
                        { "localField", "LocationId" },
                        { "foreignField", "_id" },
                        { "as", "Location" }
                    }
                }
            },
            new()
            {
                { "$unwind", "$Location" } 
            },
            new()
            {
                { "$project", new BsonDocument
                    {
                        { "Id", 1 },
                        { "PlaceType", 1 },
                        { "Location", 1 },
                        { "CreatorId", 1 },
                        { "Name", 1 },
                        { "Description", 1 },
                        { "Url", 1 },
                        { "Mail", 1 },
                        { "Polygon", 1 },
                        { "Capacity", 1 },
                        { "ImageUrls", 1 }
                    }
                }
            }
        };
        
        var result = await _mongoContext.Places.Aggregate<BsonDocument>(pipeline).ToListAsync();
        var places = result.Select(doc => BsonSerializer.Deserialize<PlaceWithLocation>(doc)).ToList();

        return places;
    }
}