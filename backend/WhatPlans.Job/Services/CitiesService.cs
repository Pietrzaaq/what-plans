using System.Globalization;
using Newtonsoft.Json;

namespace WhatPlans.Job.Services;

public class CitiesService
{
    public List<City> LoadCities(string path)
    {
        var jsonReader = new JsonTextReader(new StreamReader(path));
        var result = new JsonSerializer().Deserialize<List<City>>(jsonReader);
        return result;
    }
    
    public List<CityWithRadius> LoadCitiesWithRadius(string path)
    {
        var cities = new List<CityWithRadius>();

        var lines = File.ReadAllLines(path);
        
        for (int i = 1; i < lines.Length; i++)
        {
            var columns = lines[i].Split('\t');
            
            if (columns.Length < 6)
                continue; 

            var city = new CityWithRadius
            {
                Name = columns[0],
                District = columns[1],
                Province = columns[2],
                Area = double.Parse(columns[3], CultureInfo.InvariantCulture),
                Population = int.Parse(columns[4]),
                Density = int.Parse(columns[5])
            };

            cities.Add(city);
        }

        foreach (var city in cities)
        {
            if (city.Area < 1)
                continue;
            
            city.Radius = CalculateRadius(city.Area);
        }
        
        return cities;
    }
    
    public static int CalculateRadius(double areaInHectares)
    {
        // Convert area from hectares to square meters
        double areaInSquareMeters = areaInHectares * 10000;

        // Calculate the radius in meters
        double radius = Math.Sqrt(areaInSquareMeters / Math.PI);
        return (int) Math.Floor(radius); // Radius in meters
    }
    
    public class CityWithRadius
    {
        public string Name { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public double Area { get; set; } // in hectares
        public int Population { get; set; }
        public int Density { get; set; } // population per km²
        public int Radius { get; set; }
    }

    public class City
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Commune { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}