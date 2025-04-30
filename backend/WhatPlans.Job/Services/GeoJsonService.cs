using Geohash;
using MongoDB.Bson;
using NetTopologySuite.Features;
using NetTopologySuite.IO;
using Newtonsoft.Json.Linq;
using WhatPlans.Domain.Entities;
using WhatPlans.Domain.Enums;

namespace WhatPlans.Job.Services;

public class GeoJsonService
{
    private readonly ILogger<Worker> _logger;
    private static readonly Random _random = new Random();

    public GeoJsonService(ILogger<Worker> logger)
    {
        _logger = logger;
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
    
    public async Task<List<Place>> ParseGeoJsonFile(string filePath, List<CitiesService.City> cities)
    {
        var geoJson = File.ReadAllText(filePath);
        
        var geoJsonReader = new GeoJsonReader();
        FeatureCollection featureCollection = geoJsonReader.Read<FeatureCollection>(geoJson);

        var places = new List<Place>();

        var counter = 0;
        foreach (var feature in featureCollection)
        {
            counter++;
            _logger.LogInformation("Parsing place number: {counter}", counter);
            
            var properties = feature.Attributes;
            string cityName = properties.GetOptionalValue("addr:city") as string;

            if (cityName == null)
            {
                var fileName = Path.GetFileName(filePath);
                cityName = Path.GetFileNameWithoutExtension(fileName);
            }
            
            var city = cities.FirstOrDefault(c => c.Name == cityName);

            var geometry = feature.Geometry;
            var centroid = geometry.Centroid; 
            
            var description = properties.GetOptionalValue("heritage") is null ? properties.GetOptionalValue("building") as string : properties.GetOptionalValue("heritage") as string;
            var amenity = properties.GetOptionalValue("tourism") is null ? properties.GetOptionalValue("amenity") as string : properties.GetOptionalValue("tourism") as string;
            var street = properties.GetOptionalValue("addr:street") as string;
            var houseNumber = properties.GetOptionalValue("addr:housenumber") as string;
            var imageUrls = new List<string>();
            
            var wikidata = properties.GetOptionalValue("wikidata") as string;
            if (wikidata != null)
            {
                var imageUrl = await GetImageUrlFromWikidata(wikidata);
                
                if (imageUrl != null)
                    imageUrls.Add(imageUrl);
            }
            
            var place = new Place
            {
                Id = ObjectId.GenerateNewId(),
                CreatorId = "OpenStreetMap",
                PlaceType = MapPlaceType(properties),
                Name = properties.GetOptionalValue("name") as string,
                Description = description,
                Polygon = geometry.ToString(), 
                Url = properties.GetOptionalValue("website") as string,
                OpenStreetMapId = properties.GetOptionalValue("@id") as string,
                OpenStreetMapSport = properties.GetOptionalValue("sport") as string,
                OpenStreetMapPhone = properties.GetOptionalValue("phone") as string,
                OpenStreetMapAmenity = amenity,
                Location = new Location
                {
                    Name = properties.GetOptionalValue("name") as string,
                    Latitude = centroid.Y,
                    Longitude = centroid.X,
                    Address = $"{street} {houseNumber}",
                    CityName = city?.Name,
                    CityId = city?.Id,
                    ProvinceName = city?.Province
                },
                UpdateDate = DateTime.UtcNow,
                ImageUrls = imageUrls
            };
            
            _logger.LogInformation("Pared place number: {counter}, Name: {name}", counter, place.Name);

            places.Add(place);
        }

        return places;
    }

    private static PlaceTypes MapPlaceType(dynamic properties)
    {
        if (properties.GetOptionalValue("tourism") == "museum")
            return PlaceTypes.Museum;
        if (properties.GetOptionalValue("amenity") == "theatre")
            return PlaceTypes.Theatre;
        if (properties.GetOptionalValue("leisure") == "sports_centre")
            return PlaceTypes.SportsCentre;
        if (properties.GetOptionalValue("amenity") == "nightclub")
            return PlaceTypes.Club;

        return PlaceTypes.Other;
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
    
}