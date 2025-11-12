using Geohash;
using MongoDB.Bson;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json.Linq;
using WhatPlans.Domain.Entities;
using WhatPlans.Domain.Enums;
using WhatPlans.Domain.Mapper;
using Location = WhatPlans.Domain.Entities.Location;

namespace WhatPlans.Job.Services;

public class GeoJsonService
{
    private readonly ILogger<Worker> _logger;
    private readonly MongoService _mongoService;
    private static readonly Random _random = new Random();

    public GeoJsonService(ILogger<Worker> logger,
        MongoService mongoService)
    {
        _logger = logger;
        _mongoService = mongoService;
    }
    
    public List<City> LoadCitiesFromOsm(string filePath)
    {
        var geohasher = new Geohasher();
        var geoJson = File.ReadAllText(filePath);
        
        var geoJsonReader = new GeoJsonReader();
        FeatureCollection featureCollection = geoJsonReader.Read<FeatureCollection>(geoJson);
        
        var cities = new List<City>();
        
        var counter = 0;
        foreach (var feature in featureCollection)
        {
            counter++;
            
            _logger.LogInformation("Parsing city number: {counter}", counter);
            var properties = feature.Attributes;
            string cityName = properties.GetOptionalValue("name") as string;
            
            if (cityName == null)
                continue;
            
            var geometry = feature.Geometry;
            var centroid = geometry.Centroid; 

            var geohash = geohasher.Encode(centroid.Y, centroid.X, 4);
            var hasPopulation = int.TryParse(properties.GetOptionalValue("population") as string, out int populationInt);
            
            var city = new City
            {
                Id = ObjectId.GenerateNewId(),
                Name = cityName,
                Geohash = geohash,
                Latitude = centroid.Y,
                Longitude = centroid.X,
                Population = hasPopulation ? populationInt : null,
                OpenStreetMapId = properties.GetOptionalValue("@id") as string,
                Radius = EstimateRadius(populationInt)
            };
            
            cities.Add(city);
        }

        return cities;
    }
    
    public static int? EstimateRadius(int population, double density = 3000.0)
    {
        if (population <= 0 || density <= 0)
        {
            return null;
        }
        
        
        double radius = Math.Sqrt(population / (Math.PI * density));
        return (int) Math.Floor(radius); 
    }
    
    public async Task<List<Place>> ParseGeoJsonFile(string filePath)
    {
        var citiesDb = await _mongoService.GetCities();
        var placesDb = await _mongoService.GetPlaces();
        
        var geoJson = File.ReadAllText(filePath);

        var geohasher = new Geohasher();
        var geoJsonReader = new GeoJsonReader();
        FeatureCollection featureCollection = geoJsonReader.Read<FeatureCollection>(geoJson);

        var places = new List<Place>();
        var skippedPlacesCount = 0; 
        var samePlacesCount = 0; 

        var counter = 0;
        foreach (var feature in featureCollection)
        {
            counter++;
            _logger.LogInformation("Parsing place number: {counter}", counter);
            
            var properties = feature.Attributes;
            string cityName = properties.GetOptionalValue("addr:city") as string;
            
            var geometry = feature.Geometry;
            var centroid = geometry.Centroid; 
            string geohash = geohasher.Encode(centroid.Y, centroid.X, 9);

            if (cityName == null)
            {
                // string geohash = geohasher.Encode(centroid.Y, centroid.X, 9);
                skippedPlacesCount++;
                // var fileName = Path.GetFileName(filePath);
                // cityName = Path.GetFileNameWithoutExtension(fileName);
                continue;
            }

            var city = citiesDb.FirstOrDefault(c => c.Name == cityName);

            var openStreetMapId = properties.GetOptionalValue("@id") as string;
            var wikidataId = properties.GetOptionalValue("wikidata") as string;
            var description = properties.GetOptionalValue("heritage") is null ? properties.GetOptionalValue("building") as string : properties.GetOptionalValue("heritage") as string;
            var street = properties.GetOptionalValue("addr:street") as string;
            var houseNumber = properties.GetOptionalValue("addr:housenumber") as string;
            var imageUrls = new List<string>();
            
            if (placesDb.Any(p => p.Location.Geohash.Equals(geohash) && p.OpenStreetMapId == openStreetMapId))
            {
                samePlacesCount++;
                continue;
            }
            
            // if (wikidata != null)
            // {
            //     var imageUrl = await GetImageUrlFromWikidata(wikidata);
            //     
            //     if (imageUrl != null)
            //         imageUrls.Add(imageUrl);
            // }
            
            var place = new Place
            {
                Id = ObjectId.GenerateNewId(),
                CreatorId = "OpenStreetMap",
                PlaceType = PlaceTypes.Other,
                Name = properties.GetOptionalValue("name") as string,
                Description = description,
                Polygon = geometry.ToString(), 
                Url = properties.GetOptionalValue("website") as string,
                OpenStreetMapId = openStreetMapId,
                OpenStreetMapSport = properties.GetOptionalValue("sport") as string,
                OpenStreetMapPhone = properties.GetOptionalValue("phone") as string,
                OpenStreetMapAmenity = properties.GetOptionalValue("amenity") as string,
                OpenStreetMapBuilding = properties.GetOptionalValue("building") as string,
                OpenStreetMapHistoric = properties.GetOptionalValue("historic") as string,
                OpenStreetMapTourism = properties.GetOptionalValue("tourism") as string,
                OpenStreetMapLeisure = properties.GetOptionalValue("leisure") as string,
                OpenStreetMapWikidataId = wikidataId,
                Location = new Location
                {
                    Name = properties.GetOptionalValue("name") as string,
                    Latitude = centroid.Y,
                    Longitude = centroid.X,
                    Address = $"{street} {houseNumber}",
                    CityName = city?.Name,
                    CityId = city?.Id.ToString(),
                    ProvinceName = city?.Province,
                    Geohash = city?.Geohash,
                },
                UpdateDate = DateTime.UtcNow,
                // ImageUrls = imageUrls,
            };
            
            _logger.LogInformation("Pared place number: {counter}, Name: {name}", counter, place.Name);

            places.Add(place);
        }

        _logger.LogInformation("Skipped places: {skippedPlacesCount}", skippedPlacesCount);
        _logger.LogInformation("Same places: {samePlacesCount}", samePlacesCount);
        
        return places;
    }
    
    public async Task<string> GetImageUrlFromWikidata(string wikidataId)
    {
        var wikidataApiUrl = $"https://www.wikidata.org/wiki/Special:EntityData/{wikidataId}.json";
    
        using (var httpClient = new HttpClient())
        {
            _logger.LogInformation("Fetching data from {url} at {time}", wikidataApiUrl, DateTime.UtcNow);
            var response = await httpClient.GetStringAsync(wikidataApiUrl);
        
            var json = JObject.Parse(response);
        
            var entityData = json["entities"]?[wikidataId]?["claims"]?["P18"];
            if (entityData != null)
            {
                var imageFileName = entityData[0]?["mainsnak"]?["datavalue"]?["value"]?.ToString();
                if (!string.IsNullOrEmpty(imageFileName))
                {
                    // The image URL follows a specific pattern on Wikimedia Commons
                    var imageUrl = $"https://commons.wikimedia.org/wiki/Special:FilePath/{imageFileName}";
                    return imageUrl;
                }
            }
            
            int delayTime = _random.Next(100, 501);
            await Task.Delay(delayTime);
        }
    
        return null; // Return null if no image is found
    }

    public async Task UpdatePlaces(string filePath)
    {
        var citiesDb = await _mongoService.GetCities();
        var placesDb = await _mongoService.GetPlaces();
        
        var geoJson = File.ReadAllText(filePath);

        var geohasher = new Geohasher();
        var geoJsonReader = new GeoJsonReader();
        FeatureCollection featureCollection = geoJsonReader.Read<FeatureCollection>(geoJson);
        
        var counter = 0;
        var notFoundCount = 0;
        var deletedCount = 0;
        foreach (var feature in featureCollection)
        {
            _logger.LogInformation("Parsing city number: {counter}", counter);
            var properties = feature.Attributes;
            
            var openStreetMapId = properties.GetOptionalValue("@id") as string;
            string cityName = properties.GetOptionalValue("name") as string;
            
            if (cityName == null)
                continue;
            
            var placeDb = placesDb.FirstOrDefault(p => p.OpenStreetMapId == openStreetMapId);
            if (placeDb == null)
            {
                notFoundCount++;
                continue;
            }

            if (string.IsNullOrEmpty(placeDb.Name))
            {
                await _mongoService.DeletePlace(placeDb);
                deletedCount++;
            }
            
            var geometry = feature.Geometry;
            var centroid = geometry.Centroid; 
            string geohash = geohasher.Encode(centroid.Y, centroid.X, 9);
            var city = citiesDb.FirstOrDefault(c => c.Name == cityName);
            
            var street = properties.GetOptionalValue("addr:street") as string;
            var houseNumberStr = properties.GetOptionalValue("addr:housenumber") as string;
            int.TryParse(houseNumberStr, out int houseNumber);
            
            var wikidataId = properties.GetOptionalValue("wikidata") as string;
            var sport = properties.GetOptionalValue("sport") as string;
            var phone = properties.GetOptionalValue("phone") as string;
            var amenity = properties.GetOptionalValue("amenity") as string;
            var building = properties.GetOptionalValue("building") as string;
            var historic = properties.GetOptionalValue("historic") as string;
            var tourism = properties.GetOptionalValue("tourism") as string;
            var leisure = properties.GetOptionalValue("leisure") as string;
                
            if (city != null)
            {
                placeDb.Location.CityId = city.Id.ToString();
            }
            
            placeDb.Location.Geohash = geohash;
            placeDb.Location.FormatedAddress = $"{cityName}, {street} {houseNumber}";
            placeDb.Location.HouseNumber = houseNumber;
            
            placeDb.OpenStreetMapAmenity = amenity;
            placeDb.OpenStreetMapBuilding = building;
            placeDb.OpenStreetMapHistoric = historic;
            placeDb.OpenStreetMapLeisure = leisure;
            placeDb.OpenStreetMapPhone = phone;
            placeDb.OpenStreetMapTourism = tourism;
            placeDb.OpenStreetMapSport = sport;
            placeDb.OpenStreetMapWikidataId = wikidataId;
            
            placeDb.UpdateDate = DateTime.UtcNow;
            placeDb.CreateDate = DateTime.UtcNow;

            var (type, category) = PlaceMapper.MapPlaceTypeAndCategory(placeDb);
            placeDb.PlaceType = type;
            placeDb.PlaceCategory = category;
            
            await _mongoService.UpdatePlace(placeDb);
            _logger.LogInformation("Updated place: {name}", placeDb.Name);
            counter++;
        }
        
        _logger.LogInformation($"Finished updating places, {counter}, notFound {notFoundCount}, deleted {deletedCount}");
    }

    public async Task CleanupPlaces()
    {
        var placesDb = await _mongoService.GetPlaces();

        foreach (var place in placesDb)
        {
            (var placeType, var placeCategory) = PlaceMapper.MapPlaceTypeAndCategory(place);
            
            place.PlaceCategory = placeCategory;
            place.PlaceType = placeType;
            place.UpdateDate = DateTime.UtcNow;
            place.UpdatedBy = "Admin";
        }
    }

    public async Task UpdatePlacesCityId()
    {
        var citiesDb = await _mongoService.GetCities();
        var placesDb = await _mongoService.GetPlaces();

        var citiySetCount = 0;
        var citiyNotFoundCount = 0;
        var cityUpdatedCount = 0;
        var count = 0;
        
        foreach (var place in placesDb)
        {
            Console.WriteLine($"Updating place count: {count}");
            var city = citiesDb.FirstOrDefault(c => c.Name.ToLower() == place.Location.CityName.ToLower());

            if (city == null)
            {
                var placePoint = new Point(new Coordinate(place.Location.Longitude, place.Location.Latitude));
                City? nearestCity = null;
                double shortestDistance = double.MaxValue;

                foreach (var cityDb in citiesDb)
                {
                    var cityPoint = new Point(new Coordinate(cityDb.Longitude, cityDb.Latitude));
                    var distance = placePoint.Distance(cityPoint); 
                    
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        nearestCity = cityDb;
                    }
                }

                if (nearestCity != null)
                {
                    place.Location.CityId = nearestCity.Id.ToString();
                    place.Location.CityName = nearestCity.Name;
                    citiySetCount++;
                }
                else
                    citiyNotFoundCount++;
            }
            else
            {
                place.Location.CityId = city.Id.ToString();

                if (place.Location.HouseNumber == 0)
                {
                    place.Location.FormatedAddress = $"{city.Name}, {place.Name}";
                    place.Location.HouseNumber = null;
                }
                
                cityUpdatedCount++;
            }

            count++;
        }
        
        Console.WriteLine($"{citiySetCount} cities updated, {citiyNotFoundCount} cities not found, {cityUpdatedCount} cities updated");
    }
}