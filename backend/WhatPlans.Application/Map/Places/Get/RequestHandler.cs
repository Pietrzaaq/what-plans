using MediatR;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Map.Places.Get;

public class RequestHandler : IRequestHandler<Request, List<Place>>
{
    private readonly IMongoContext _mongoContext;

    public RequestHandler(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }
    
    public async Task<List<Place>> Handle(Request request, CancellationToken cancellationToken)
    {
        var geohash = request.Geohash;

        var geohashPlaces = await _mongoContext.Places
            .Find(p => p.Location.Geohash.Contains(geohash))
            .ToListAsync(cancellationToken: cancellationToken);

        var result = geohashPlaces
            .Where(p =>
                p.Location.Latitude < request.North &&
                p.Location.Latitude > request.South &&
                p.Location.Longitude < request.West &&
                p.Location.Longitude > request.East)
            .Take(GetResultLimit(geohash.Length))
            .ToList();

        return result;
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