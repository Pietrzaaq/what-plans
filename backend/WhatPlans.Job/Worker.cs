using MongoDB.Bson;
using WhatPlans.Domain.Entities;
using WhatPlans.Job.Services;

namespace WhatPlans.Job;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly CitiesService _citiesService;
    private readonly GeoJsonService _geoJsonService;
    private readonly MongoService _mongoService;
    private readonly ImageService _imageService;
    private readonly PlacesCleanupService _placesCleanupService;

    public Worker(ILogger<Worker> logger, CitiesService citiesService, GeoJsonService geoJsonService, MongoService mongoService, ImageService imageService, PlacesCleanupService placesCleanupService)
    {
        _logger = logger;
        _citiesService = citiesService;
        _geoJsonService = geoJsonService;
        _mongoService = mongoService;
        _imageService = imageService;
        _placesCleanupService = placesCleanupService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker starting at: {time}", DateTimeOffset.Now);

        // await UpdatePlaces();

        // await LoadCitiesFromJsonFiles();

        // await AddGeohashToPlaces();
        //
        // await AddCitiesToDatabase();

        // await SetCityRadius();

        // await MigrateImages();

        // await _placesCleanupService.CleanupPlaces();

        await _mongoService.MigrateEnumsToStrings();
        _logger.LogInformation("Worker finished job at: {time}", DateTimeOffset.Now);
    }

    private async Task MigrateImages()
    {
        var places = await _mongoService.GetPlaces();

        foreach (var place in places)
        {
            var imagesUrl = place.ImageUrls;
            
            if (imagesUrl == null || !imagesUrl.Any())
                continue;
            
            var images = new List<Image>();

            foreach (var url in imagesUrl)
            {
                try
                {
                    Console.WriteLine($"Fetching for: {place.Name}, url: {url}");
                    
                    var image = await _imageService.SaveImageFromUrlAsync(url, place.Id, "Place",
                        $"Image of place: ${place.Name}", ObjectId.Empty, $"Image of place: ${place.Name}");
                    images.Add(image);

                    Console.WriteLine($"Image of place: ${place.Name}, succesfully stored, image url: {url}, image size: {image.Size}, image format {image.Format}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(
                        $"Failed to fetch image for: {place.Name}({place.Id}), error message: {ex.Message}");
                }
                finally
                {
                    Thread.Sleep(1500);
                }
            }

            _mongoService.AddImages(images);

            var imageIds = images.Select(i => i.Id).ToList();
            _mongoService.UpdateImages(place.Id, imageIds);
        }
    }
    
    private async Task SetCityRadius()
    {
        var cities = _citiesService.LoadCitiesWithRadius("Data/tablica.txt");

        await _mongoService.UpdateCitiesRadius(cities);
        
        _logger.LogInformation("Updated cities radius at: {time}", DateTimeOffset.Now);
    }

    private async Task AddCitiesToDatabase()
    {
        var cities = _citiesService.LoadCities("Data/cities.json");
        
        var citiesFromOsm = _geoJsonService.LoadCitiesFromOsm("Data/cities_from_osm.geojson");

        foreach (var cityOsm in citiesFromOsm)
        {
            var city = cities.FirstOrDefault(c => c.Name == cityOsm.Name);

            if (city == null)
            {
                _logger.LogInformation("Didnt found city {name}", cityOsm.Name);
                continue;
            }
            
            cityOsm.Province = city.Province;
            cityOsm.District = city.District;
            cityOsm.Commune = city.Commune;
        }

        await _mongoService.InsertCities(citiesFromOsm);
    }
    
    private async Task AddGeohashToPlaces()
    {
        var places = await _mongoService.GetPlaces();
        
        _logger.LogInformation("Loaded {places} places from database at: {time}", places.Count, DateTimeOffset.Now);
        
        await _mongoService.UpdatePlacesGeohash(places);
        
        _logger.LogInformation("Updated {places} places at: {time}", places.Count, DateTimeOffset.Now);
    }

    private async Task LoadCitiesFromJsonFiles()
    {
        // var cities = _citiesService.LoadCities("Data/cities.json");
        //
        // _logger.LogInformation("Loaded {cities} cities from cities.json at: {time}", cities.Count, DateTimeOffset.Now);
        
        var geoJsonFiles = Directory.GetFiles("Data", "export.geojson");
        foreach (var filePath in geoJsonFiles)
        {
            _logger.LogInformation("Loading places from geojson {geoJson} at: {time}", filePath, DateTimeOffset.Now);
            
            var places = await _geoJsonService.ParseGeoJsonFile(filePath);
            
            _logger.LogInformation("Loaded {places} places at: {time}", places.Count, DateTimeOffset.Now);
            
            await _mongoService.InsertPlaces(places);
            
            _logger.LogInformation("Saved places to database at: {time}", DateTimeOffset.Now);
        }
    }
    
    private async Task UpdatePlaces()
    {
        await _geoJsonService.CleanupPlaces();
        // await _geoJsonService.UpdatePlacesCityId();


        // var geoJsonFiles = Directory.GetFiles("Data", "export.geojson");
        // foreach (var filePath in geoJsonFiles)
        // {
        //     await _geoJsonService.UpdatePlaces(filePath);
        // }
    }
}