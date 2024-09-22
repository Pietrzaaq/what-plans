using MongoDB.Bson;

namespace WhatPlans.Domain.Entities;

public class Place
{
    public ObjectId Id { get; set; }
    public ObjectId CreatorId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Polygon { get; set; }
    public int Capacity { get; set; }
}