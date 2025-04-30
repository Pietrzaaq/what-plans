using Geohash;
using MongoDB.Bson;
using MongoDB.Driver;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Job.Services;

public class MongoService
{
    private readonly IMongoDatabase _database;
    
    public MongoService(IConfiguration configuration)
    {
        var connectionString = configuration.GetSection("Database:ConnectionString").Value;
        var databaseName = configuration.GetSection("Database:DatabaseName").Value;
        
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }
    
    public async Task InsertPlaces(List<Place> places)
    {
        var placesCollection = _database.GetCollection<Place>("Places");
        
        await placesCollection.InsertManyAsync(places);
    }
    
    public async Task<List<Place>> GetPlaces()
    {
        var placesCollection = _database.GetCollection<Place>("Places");
        
        return await placesCollection.Find(_ => true).ToListAsync();
    }

    public async Task UpdatePlacesGeohash(List<Place> places)
    {
        var geohasher = new Geohasher();
        var placesCollection = _database.GetCollection<Place>("Places");

        foreach (var place in places)
        {
            if (place.Location != null && place.Location.Latitude != 0 && place.Location.Longitude != 0)
            {
                string geohash = geohasher.Encode(place.Location.Latitude, place.Location.Longitude, 9);

                var updateDefinition = Builders<Place>.Update.Set(p => p.Location.Geohash, geohash);

                await placesCollection.UpdateOneAsync(Builders<Place>.Filter.Eq(p => p.Id, place.Id), updateDefinition);
            }
        }
    }

    public async Task InsertCities(List<City> citiesFromOsm)
    {
        var citiesCollection = _database.GetCollection<City>("Cities");

        await citiesCollection.InsertManyAsync(citiesFromOsm);
    }

    public async Task UpdateCitiesRadius(List<CitiesService.CityWithRadius> citiesWithRadius)
    {
        var citiesCollection = _database.GetCollection<City>("Cities");
        var cities = citiesCollection.Find(_ => true).ToList();
        
        foreach (var city in cities)
        {
            var matchedCity = citiesWithRadius.FirstOrDefault(c => c.Name == city.Name);
            int radius;
            
            if (matchedCity == null)
                radius = 1300;
            else
                radius = matchedCity.Radius;
            
            var updateDefinition = Builders<City>.Update.Set(p => p.Radius, radius);

            await citiesCollection.UpdateOneAsync(Builders<City>.Filter.Eq(c => c.Id, city.Id), updateDefinition);
        }
    }
    
    public void AddImages(List<Image> images)
    {
        var placesCollection = _database.GetCollection<Image>("Images");
        
        placesCollection.InsertManyAsync(images);
        
        Console.WriteLine($"{images.Count} images added");
    }

    public void UpdateImages(ObjectId placeId, List<ObjectId> imageIds)
    {
        var placesCollection = _database.GetCollection<Place>("Places");
        var updateDefinition = Builders<Place>.Update.Set(p => p.ImageIds, imageIds);
        
        placesCollection.UpdateOneAsync(Builders<Place>.Filter.Eq(c => c.Id, placeId), updateDefinition);
    }
}