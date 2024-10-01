using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WhatPlans.Domain.Common;
using WhatPlans.Domain.Enums;

namespace WhatPlans.Domain.Entities;

[BsonIgnoreExtraElements]
public class PlaceWithEvents
{
    public ObjectId Id { get; set; }
    [BsonRepresentation(BsonType.String)]
    public PlaceTypes Type { get; set; }       
    public string CreatorId { get; set; }
    public string Name { get; set; }
    public Location Location { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }
    public string Mail { get; set; }
    public string Polygon { get; set; }
    public int? Capacity { get; set; }
    public List<string> ImageUrls { get; set; }
    public List<Event> Events { get; set; }
}