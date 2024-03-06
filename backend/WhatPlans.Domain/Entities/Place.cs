namespace WhatPlans.Domain.Entities;

public class Place
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public string Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Polygon { get; set; }
}