using WhatPlans.Domain.Common;
using WhatPlans.Domain.ValueObjects;

namespace WhatPlans.Domain.Entities;

public class Event
{
    public EntityId Id { get; set; }
    public EntityId PlaceId { get; set; }
    public EntityId CreatorId { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public EventTypes Type { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public TimeSpan Duration { get; set; }
    public EntityId[] ImageIds { get; set; }
}