using MediatR;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Events.Get;

public class GetEventsRequestHandler : IRequestHandler<GetEventsRequest, List<Event>>
{
    private readonly IMongoContext _mongoContext;

    public GetEventsRequestHandler(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }
    
    public async Task<List<Event>> Handle(GetEventsRequest request, CancellationToken cancellationToken)
    {
        var result = await _mongoContext.Events.Find(p => true).ToListAsync(cancellationToken: cancellationToken);

        return result;
    }
}