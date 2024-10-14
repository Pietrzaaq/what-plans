using MongoDB.Bson;

namespace WhatPlans.Domain.Entities;

public class City
{
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public string Geohash { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Province { get; set; }
    public string District { get; set; }
    public string Commune { get; set; }
    public string OpenStreetMapId { get; set; }
    public int? Population { get; set; }
    public int? Radius { get; set; }
}