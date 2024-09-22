using WhatPlans.Domain.Common;

namespace WhatPlans.Domain.Entities;

public class Artist
{
    public EntityId Id { get; set; }
    public EntityId UserId { get; set; }
    public string Name { get; set; }
}