using MongoDB.Driver;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Infrastructure.Database;

public class MongoContext : IMongoContext
{
    public IMongoDatabase Database { get; }

    public MongoContext(MongoDbSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        Database = client.GetDatabase(settings.DatabaseName) ;
    }

    public IMongoCollection<Place> Places => Database.GetCollection<Place>("Places");
    public IMongoCollection<Event> Events => Database.GetCollection<Event>("Events");
    public IMongoCollection<Location> Locations => Database.GetCollection<Location>("Locations");
    public IMongoCollection<City> Cities => Database.GetCollection<City>("Cities");
    public IMongoCollection<User> Users => Database.GetCollection<User>("Users");
}