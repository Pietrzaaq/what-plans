using MediatR;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Map.Cities.Get;

public class RequestHandler : IRequestHandler<Request, List<City>>
{
    private readonly IMongoContext _mongoContext;

    public RequestHandler(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }
    
    public async Task<List<City>> Handle(Request request, CancellationToken cancellationToken)
    {
        var geohashes = request.Geohashes;
        
        var result = await _mongoContext.Cities.Find(c => geohashes.Any(g => c.Geohash.Contains(g))).ToListAsync(cancellationToken: cancellationToken);

        return result;
    }
}