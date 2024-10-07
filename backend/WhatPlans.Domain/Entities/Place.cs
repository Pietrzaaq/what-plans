using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WhatPlans.Domain.Enums;

namespace WhatPlans.Domain.Entities;

public class Place
{
    public ObjectId Id { get; set; }
    public PlaceTypes PlaceType { get; set; }
    public ObjectId LocationId { get; set; }
    public string CreatorId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }
    public string Mail { get; set; }
    public string Polygon { get; set; }
    public int? Capacity { get; set; }
    public List<string> ImageUrls { get; set; }
}

[BsonIgnoreExtraElements]
public class PlaceWithLocation : Place
{
    public Location Location { get; set; }
}

[BsonIgnoreExtraElements]
public class PlaceWithEvents : PlaceWithLocation
{
    public List<Event> Events { get; set; }
}

