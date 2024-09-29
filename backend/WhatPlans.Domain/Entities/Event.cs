using MongoDB.Bson;

using WhatPlans.Domain.ValueObjects;

namespace WhatPlans.Domain.Entities;

public class Event
{
    public ObjectId Id { get; set; }
    public ObjectId PlaceId { get; set; }
    public ObjectId CreatorId { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public EventTypes Type { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public TimeSpan Duration { get; set; }
    public ObjectId[] ImageIds { get; set; }
}