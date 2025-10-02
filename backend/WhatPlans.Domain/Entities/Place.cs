using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WhatPlans.Domain.Enums;

namespace WhatPlans.Domain.Entities;

public class Place
{
    public ObjectId Id { get; set; }
    public PlaceTypes PlaceType { get; set; }
    public PlaceCategory PlaceCategory { get; set; }
    public Location Location { get; set; }
    public string CreatorId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }
    public string Mail { get; set; }
    public string Polygon { get; set; }
    public int? Capacity { get; set; }
    public List<string> ImageUrls { get; set; }
    public List<ObjectId> ImageIds { get; set; }
    public string OpenStreetMapId { get; set; }
    public string OpenStreetMapAmenity { get; set; }
    public string OpenStreetMapSport { get; set; }
    public string OpenStreetMapPhone { get; set; }
    public string OpenStreetMapWikidataId { get; set; }
    public string OpenStreetMapBuilding { get; set; }
    public string OpenStreetMapHistoric { get; set; }
    public string OpenStreetMapLeisure { get; set; }
    public string OpenStreetMapTourism { get; set; }
    public string GoogleMapsUrl { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
}


[BsonIgnoreExtraElements]
public class PlaceWithEvents : Place
{
    public List<Event> Events { get; set; }
}

