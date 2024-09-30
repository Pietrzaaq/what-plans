using MongoDB.Bson;
using WhatPlans.Domain.Common;
using WhatPlans.Domain.Enums;

namespace WhatPlans.Domain.Entities;

public class Place
{
    public ObjectId Id { get; set; }
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
}