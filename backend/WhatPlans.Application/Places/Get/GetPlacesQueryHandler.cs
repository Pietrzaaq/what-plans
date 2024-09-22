using MediatR;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Places.Get;

public class GetPlacesQueryHandler : IRequestHandler<GetPlacesQuery, List<Place>>
{
    private readonly IMongoContext _mongoContext;

    public GetPlacesQueryHandler(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }
    
    public async Task<List<Place>> Handle(GetPlacesQuery request, CancellationToken cancellationToken)
    {
        var result = await _mongoContext.Places.Find(p => true).ToListAsync(cancellationToken: cancellationToken);

        return result;
    }
}