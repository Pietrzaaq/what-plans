using MediatR;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Places.Get.ByIdWithEvents;

public class RequestHandler : IRequestHandler<Request, List<Event>>
{
    private readonly IMongoContext _mongoContext;

    public RequestHandler(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }
    
    public async Task<List<Event>> Handle(Request request, CancellationToken cancellationToken)
    {
        var events = await _mongoContext.Events.Find(p => p.PlaceId == request.Id).ToListAsync(cancellationToken: cancellationToken);

        return events;
    }
}