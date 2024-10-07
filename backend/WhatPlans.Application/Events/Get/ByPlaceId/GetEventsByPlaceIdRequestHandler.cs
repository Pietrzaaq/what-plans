using MediatR;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Events.Get.ByPlaceId;

public class GetEventsByPlaceIdRequestHandler : IRequestHandler<GetEventsByPlaceIdRequest, List<Event>>
{
    private readonly IMongoContext _mongoContext;

    public GetEventsByPlaceIdRequestHandler(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }
    
    public async Task<List<Event>> Handle(GetEventsByPlaceIdRequest request, CancellationToken cancellationToken)
    {
        var result = await _mongoContext.Events.Find(p => p.PlaceId == request.PlaceId).ToListAsync(cancellationToken);

        return result;
    }
}