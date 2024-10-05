using MongoDB.Bson;

namespace WhatPlans.Domain.Entities;

public class Location
{
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Address { get; set; }
    public string FormatedAddress { get; set; }
    public string CityName { get; set; }
    public string CityId { get; set; }
    public string ProvinceName { get; set; } 
    public string OpenStreetMapId { get; set; }
    public string GoogleMapsUrl { get; set; }
}