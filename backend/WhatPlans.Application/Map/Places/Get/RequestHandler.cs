using System.Diagnostics;
using Geohash;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Map.Places.Get;

public class RequestHandler : IRequestHandler<Request, List<Place>>
{
    private readonly IMongoContext _mongoContext;
    private readonly ILogger<RequestHandler> _logger;

    public RequestHandler(IMongoContext mongoContext, ILogger<RequestHandler> logger)
    {
        _mongoContext = mongoContext;
        _logger = logger;
    }
    
    public async Task<List<Place>> Handle(Request request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Get places request received. Geohash: {request.Geohash}, BBox: [{request.North}, {request.West}], [{request.South}, {request.East}]");
        
        var sw = new Stopwatch();
        sw.Start();
        
        var geohash = request.Geohash;
        var bbox = new BoundingBox()
        {
            MaxLat = request.North,
            MinLat = request.South,
            MaxLng = request.East,
            MinLng = request.West
        };
        List<Place> places;
        
        var geohasher = new Geohasher();
        var neighbourGeohashes = geohasher
            .GetNeighbors(geohash)
            .Select(g => g.Value)
            .ToList();
        
        Dictionary<string, BoundingBox> neighbourGeohashesBBoxes = neighbourGeohashes
            .ToDictionary(g => g, g => geohasher.GetBoundingBox(g));
        var visibleNeighbourGeohashes = neighbourGeohashesBBoxes
            .Where(b => IsOverlapping(b.Value, bbox))
            .ToList()
            .Select(b => b.Key)
            .ToList();
        
        _logger.LogInformation($"Calculated neighbours geohashes in {sw.ElapsedMilliseconds} ms. Visible neighbours: {String.Join(", ", visibleNeighbourGeohashes)}");
        sw.Restart();

        if (visibleNeighbourGeohashes.Any())
        {
            var geohashes = visibleNeighbourGeohashes.Union(new List<string> {geohash});
            var pattern = $"^({string.Join("|", geohashes)})";

            var filter = Builders<Place>.Filter.Regex(
                p => p.Location.Geohash,
                new MongoDB.Bson.BsonRegularExpression(pattern)
            );

            places = await _mongoContext.Places
                .Find(filter)
                .ToListAsync(cancellationToken);
        }
        else
        {
            places = await _mongoContext.Places
                .Find(p => p.Location.Geohash.StartsWith(geohash))
                .ToListAsync(cancellationToken: cancellationToken);
        }

        _logger.LogInformation($"Filtered places by geohashes in {sw.ElapsedMilliseconds} ms.");
        sw.Restart();

        var result = places
            .Where(p =>
                p.Location.Latitude < request.North &&
                p.Location.Latitude > request.South &&
                p.Location.Longitude > request.West &&
                p.Location.Longitude < request.East)
            .Take(GetResultLimit(geohash.Length))
            .ToList();

        _logger.LogInformation($"Filtered places by bbox in {sw.ElapsedMilliseconds} ms.");
        
        return result;
    }

    private bool IsOverlapping(BoundingBox bbox1, BoundingBox bbox2)
    {
        if (bbox1.MaxLat < bbox2.MinLat || bbox2.MaxLat <= bbox1.MinLat)
            return false;
        
        if (bbox1.MaxLng <= bbox2.MinLng || bbox2.MaxLng <= bbox1.MinLng)
            return false;
        
        return true;
    }

    private int GetResultLimit(int geohashPrecision)
    {
        return geohashPrecision switch
        {
            6 => 20,
            5 => 50,
            4 => 100,
            3 => 200,
            2 => 500,
            _ => 1000
        };
    }
}