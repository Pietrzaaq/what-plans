using MediatR;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Events.Get;

public class GetEventsRequestHandler : IRequestHandler<GetEventsRequest, List<EventWithLocation>>
{
    private readonly IMongoContext _mongoContext;

    public GetEventsRequestHandler(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }
    
    public async Task<List<EventWithLocation>> Handle(GetEventsRequest request, CancellationToken cancellationToken)
    {
        
        var filter = request.PlaceId.HasValue 
            ? Builders<Event>.Filter.Eq(p => p.Id, request.PlaceId.Value)
            : Builders<Event>.Filter.Empty; // If no PlaceId is provided, get all places

        var result = await _mongoContext.Events.Aggregate()
            .Match(filter)
            .Lookup<Event, Location, EventWithLocation>(
                _mongoContext.Locations,
                p => p.LocationId,
                l => l.Id, 
                p => p.Location 
            )
            .Unwind<EventWithLocation, EventWithLocation>(e => e.Location)
            .ToListAsync(cancellationToken: cancellationToken);
        
        // var pipeline = new BsonDocument[]
        // {
        //     new BsonDocument
        //     {
        //         { "$match", new BsonDocument("_id", placeId) } // Filter by Place Id
        //     },
        //     new()
        //     {
        //         { "$lookup", new BsonDocument
        //             {
        //                 { "from", "Locations" }, // The collection to join with
        //                 { "localField", "LocationId" }, // Field in the "Place" collection
        //                 { "foreignField", "_id" }, // Field in the "Location" collection
        //                 { "as", "Location" } // The result will be stored in the "Location" field
        //             }
        //         }
        //     },
        //     new()
        //     {
        //         { "$unwind", "$Location" } 
        //     },
        //     new()
        //     {
        //         { "$project", new BsonDocument
        //             {
        //                 { "Id", 1 },
        //                 { "EventType", 1 },
        //                 { "PlaceId", 1 },
        //                 { "Location", 1 }, // The joined Location object
        //                 { "CreatorId", 1 },
        //                 { "Name", 1 },
        //                 { "Url", 1 },
        //                 { "StartDate", 1 },
        //                 { "EndDate", 1 },
        //                 { "Duration", 1 },
        //                 { "ImageUrls", 1 }
        //             }
        //         }
        //     }
        // };
        //
        // var result = await _mongoContext.Events.Aggregate<BsonDocument>(pipeline).ToListAsync(cancellationToken: cancellationToken);
        // var events = result.Select(doc => BsonSerializer.Deserialize<EventWithLocation>(doc)).ToList();

        return result;
    }
}