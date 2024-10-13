using MediatR;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Places.Get.All;

public class GetPlacesRequestHandler : IRequestHandler<GetPlacesRequest, List<Place>>
{
    private readonly IMongoContext _mongoContext;

    public GetPlacesRequestHandler(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }
    
    public async Task<List<Place>> Handle(GetPlacesRequest request, CancellationToken cancellationToken)
    {
        var geohashes = request.Geohashes;
        List<Place> result;
        
        if (geohashes is null || geohashes.Count == 0)
            result = await _mongoContext.Places.Find(p => true).ToListAsync(cancellationToken: cancellationToken);
        else
            result = await _mongoContext.Places.Find(p => geohashes.Any(g => p.Location.Geohash.Contains(g))).ToListAsync(cancellationToken: cancellationToken);

        return result;
    }
}