using MediatR;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Places.Get;

public class GetPlaceByIdRequestHandler : IRequestHandler<GetPlaceByIdRequest, Place>
{
    private readonly IMongoContext _mongoContext;

    public GetPlaceByIdRequestHandler(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }
    
    public async Task<Place> Handle(GetPlaceByIdRequest request, CancellationToken cancellationToken)
    {
        var place = await _mongoContext.Places.Find(p => p.Id == request.Id).FirstOrDefaultAsync(cancellationToken);

        return place;
    }
}