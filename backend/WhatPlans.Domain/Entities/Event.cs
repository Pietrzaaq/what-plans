using WhatPlans.Domain.ValueObjects;

namespace WhatPlans.Domain.Entities;

public class Event
{
    public Guid Id { get; set; }
    public Guid PlaceId { get; set; }
    public Guid CreatorId { get; set; }
    public string Name { get; set; }
    public EventTypes Type { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public TimeSpan Duration { get; set; }
}