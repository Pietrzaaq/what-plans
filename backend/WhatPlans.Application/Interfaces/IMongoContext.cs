using MongoDB.Driver;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Interfaces;

public interface IMongoContext
{ 
    IMongoDatabase Database { get; }
    
    IMongoCollection<Place> Places { get; }
    IMongoCollection<Event> Events { get; }
    IMongoCollection<Location> Locations { get; }
}