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
        var result = await _mongoContext.Places.Find(p => true).ToListAsync(cancellationToken: cancellationToken);

        return result;
    }
}