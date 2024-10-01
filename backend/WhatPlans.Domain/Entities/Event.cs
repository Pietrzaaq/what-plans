using MongoDB.Bson;
using WhatPlans.Domain.Common;
using WhatPlans.Domain.Enums;

namespace WhatPlans.Domain.Entities;

public class Event
{
    public ObjectId Id { get; set; }
    public EventTypes EventType { get; set; }
    public ObjectId? PlaceId { get; set; }
    public string CreatorId { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public Location Location { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? Duration { get; set; }
    public List<string> ImageUrls { get; set; }
}