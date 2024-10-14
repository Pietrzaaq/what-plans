using MediatR;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Events.Get.All;

public class RequestHandler : IRequestHandler<Request, List<Event>>
{
    private readonly IMongoContext _mongoContext;

    public RequestHandler(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }
    
    public async Task<List<Event>> Handle(Request request, CancellationToken cancellationToken)
    {
        var result = await _mongoContext.Events.Find(e => true).ToListAsync(cancellationToken: cancellationToken);

        return result;
    }
}