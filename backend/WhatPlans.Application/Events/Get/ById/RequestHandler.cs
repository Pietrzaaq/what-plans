using MediatR;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Events.Get.ById;

public class RequestHandler : IRequestHandler<Request, Event>
{
    private readonly IMongoContext _mongoContext;

    public RequestHandler(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }
    
    public async Task<Event> Handle(Request request, CancellationToken cancellationToken)
    {
        var result = await _mongoContext.Events.Find(p => p.Id == request.Id).FirstOrDefaultAsync(cancellationToken);

        return result;
    }
}