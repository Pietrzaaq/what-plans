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
        var geohashes = request.Geohashes;

        var result = await _mongoContext.Places.Find(p => geohashes.Any(g => p.Location.Geohash.Contains(g))).ToListAsync(cancellationToken: cancellationToken);

        return result;
    }
}