using MediatR;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Events.Get.All;

public class GetEventsRequestHandler : IRequestHandler<GetEventsRequest, List<EventWithLocation>>
{
    private readonly IMongoContext _mongoContext;

    public GetEventsRequestHandler(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }
    
    public async Task<List<EventWithLocation>> Handle(GetEventsRequest request, CancellationToken cancellationToken)
    {
        var result = await _mongoContext.Events.Aggregate()
            .Lookup<Event, Location, EventWithLocation>(
                _mongoContext.Locations,
                p => p.LocationId,
                l => l.Id, 
                p => p.Location 
            )
            .Unwind<EventWithLocation, EventWithLocation>(e => e.Location)
            .ToListAsync(cancellationToken: cancellationToken);

        return result;
    }
}