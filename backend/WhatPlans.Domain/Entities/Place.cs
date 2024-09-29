using WhatPlans.Domain.Common;

namespace WhatPlans.Domain.Entities;

public class Place
{
    public EntityId Id { get; set; }
    public EntityId CreatorId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Polygon { get; set; }
    public int Capacity { get; set; }
    public List<EntityId> ImageIds => new List<EntityId>();
}