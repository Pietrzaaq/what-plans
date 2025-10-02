using System.Net.Http;
using System.Text.Json;
using Geohash;
using MongoDB.Bson;
using WhatPlans.Domain.Entities;
using WhatPlans.Domain.Mapper;
using WhatPlans.Job.Services;

public class PlacesCleanupService
{
    private readonly MongoService _mongoService;
    private readonly HttpClient _httpClient;
    private readonly Geohasher _geohasher;
    
    public PlacesCleanupService(MongoService mongoService, IHttpClientFactory httpClientFactory)
    {
        _mongoService = mongoService;
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "WhatPlansApp/1.0 (pietrzakd53@gmail.com)");
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        _httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
        _httpClient.Timeout = TimeSpan.FromSeconds(10);
        _geohasher = new Geohasher();
    }

    public async Task CleanupPlaces()
    {
        Console.WriteLine("Starting places cleanup...");
        
        var placesDb = await _mongoService.GetPlaces();
        var cities = await _mongoService.GetCities();
        
        Console.WriteLine($"Found {placesDb.Count} places and {cities.Count} cities");
        
        // Step 1: Update city geohashes to be more precise (6 characters recommended for city-level matching)
        await UpdateCityGeohashes(cities);
        
        int processedCount = 0;
        int addressesFetched = 0;
        int citiesMatched = 0;
        
        foreach (var place in placesDb)
        {
            bool needsUpdate = false;
            
            // Update place type and category
            (var placeType, var placeCategory) = PlaceMapper.MapPlaceTypeAndCategory(place);
            place.PlaceCategory = placeCategory;
            place.PlaceType = placeType;
            needsUpdate = true;
            
            // Fix missing or null location
            if (place.Location == null)
            {
                Console.WriteLine($"Warning: Place {place.Id} has no location, skipping...");
                continue;
            }
            
            // Step 2: Fetch missing addresses using Nominatim
            if (string.IsNullOrEmpty(place.Location.Address) || string.IsNullOrEmpty(place.Location.FormatedAddress))
            {
                var address = await FetchAddressFromNominatim(place.Location.Latitude, place.Location.Longitude);
                if (address != null)
                {
                    place.Location.Address = address.Address;
                    place.Location.FormatedAddress = address.DisplayName;
                    place.Location.CityName = address.City ?? address.Town ?? address.Village;
                    place.Location.ProvinceName = address.State;
                    
                    if (int.TryParse(address.HouseNumber, out int houseNum))
                        place.Location.HouseNumber = houseNum;
                    
                    needsUpdate = true;
                    addressesFetched++;
                    
                    await Task.Delay(1100);
                }
            }
            
            // Step 3: Match place to correct city using geohash proximity
            if (string.IsNullOrEmpty(place.Location.CityId) || !ObjectId.TryParse(place.Location.CityId, out _))
            {
                var matchedCity = FindNearestCity(place.Location, cities);
                if (matchedCity != null)
                {
                    place.Location.CityId = matchedCity.Id.ToString();
                    place.Location.CityName = matchedCity.Name;
                    place.Location.ProvinceName = matchedCity.Province;
                    needsUpdate = true;
                    citiesMatched++;
                }
            }
            
            // Update place geohash if needed (9 chars for precise location)
            if (string.IsNullOrEmpty(place.Location.Geohash))
            {
                place.Location.Geohash = _geohasher.Encode(place.Location.Latitude, place.Location.Longitude, 9);
                needsUpdate = true;
            }
            
            if (needsUpdate)
            {
                place.UpdateDate = DateTime.UtcNow;
                await _mongoService.UpdatePlace(place);
            }
            
            processedCount++;
            if (processedCount % 100 == 0)
            {
                Console.WriteLine($"Processed {processedCount}/{placesDb.Count} places...");
            }
        }
        
        Console.WriteLine($"Cleanup completed!");
        Console.WriteLine($"- Processed: {processedCount} places");
        Console.WriteLine($"- Addresses fetched: {addressesFetched}");
        Console.WriteLine($"- Cities matched: {citiesMatched}");
    }
    
    private async Task UpdateCityGeohashes(List<City> cities)
    {
        Console.WriteLine("Updating city geohashes...");
        int updated = 0;
        
        foreach (var city in cities)
        {
            // Update to 6 characters for city-level precision (~610m accuracy)
            // This is good for matching nearby places to cities
            if (string.IsNullOrEmpty(city.Geohash) || city.Geohash.Length < 6)
            {
                city.Geohash = _geohasher.Encode(city.Latitude, city.Longitude, 6);
                await _mongoService.UpdateCity(city);
                updated++;
            }
        }
        
        Console.WriteLine($"Updated {updated} city geohashes");
    }
    
    private City FindNearestCity(Location location, List<City> cities)
    {
        if (cities == null || cities.Count == 0)
            return null;
        
        // First try geohash matching (fast but approximate)
        var placeGeohash = _geohasher.Encode(location.Latitude, location.Longitude, 6);
        
        // Find cities with matching geohash prefix (same 6-char geohash = within ~610m)
        var nearestCities = cities
            .Where(c => !string.IsNullOrEmpty(c.Geohash) && placeGeohash.StartsWith(c.Geohash[..Math.Min(4, c.Geohash.Length)]))
            .ToList();
        
        // If no geohash matches, find nearest by distance
        if (nearestCities.Count == 0)
        {
            nearestCities = cities;
        }
        
        // Calculate actual distances and find the nearest
        City nearest = null;
        double minDistance = double.MaxValue;
        
        foreach (var city in nearestCities)
        {
            double distance = CalculateDistance(location.Latitude, location.Longitude, city.Latitude, city.Longitude);
            
            // Consider city radius if available
            double effectiveRadius = (city.Radius ?? 5000) / 1000.0; // Convert to km
            
            if (distance < effectiveRadius && distance < minDistance)
            {
                minDistance = distance;
                nearest = city;
            }
        }
        
        return nearest;
    }
    
    private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        // Haversine formula for distance in kilometers
        const double R = 6371; // Earth's radius in km
        
        var dLat = ToRadians(lat2 - lat1);
        var dLon = ToRadians(lon2 - lon1);
        
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        
        return R * c;
    }
    
    private double ToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }
    
    private async Task<NominatimAddress> FetchAddressFromNominatim(double latitude, double longitude)
    {
        try
        {
            var url = $"https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat={latitude:F7}&lon={longitude:F7}&addressdetails=1&zoom=18";
            
            var response = await _httpClient.GetAsync(url);
            
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to fetch address for {latitude}, {longitude}: {response.StatusCode}");
                return null;
            }
            
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<NominatimResponse>(content, new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            });
            
            if (result?.Address == null)
                return null;
            
            return new NominatimAddress
            {
                Address = result.Address.Road ?? result.Address.Suburb ?? result.Address.City,
                DisplayName = result.DisplayName,
                City = result.Address.City,
                Town = result.Address.Town,
                Village = result.Address.Village,
                State = result.Address.State,
                HouseNumber = result.Address.HouseNumber
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching address from Nominatim: {ex.Message}");
            return null;
        }
    }
}

// DTOs for Nominatim API
public class NominatimResponse
{
    public string DisplayName { get; set; }
    public NominatimAddressDetails Address { get; set; }
}

public class NominatimAddressDetails
{
    public string Road { get; set; }
    public string HouseNumber { get; set; }
    public string Suburb { get; set; }
    public string City { get; set; }
    public string Town { get; set; }
    public string Village { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public string Postcode { get; set; }
}

public class NominatimAddress
{
    public string Address { get; set; }
    public string DisplayName { get; set; }
    public string City { get; set; }
    public string Town { get; set; }
    public string Village { get; set; }
    public string State { get; set; }
    public string HouseNumber { get; set; }
}