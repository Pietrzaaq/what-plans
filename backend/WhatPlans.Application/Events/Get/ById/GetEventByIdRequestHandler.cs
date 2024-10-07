using MediatR;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Events.Get.ById;

public class GetEventByIdRequestHandler : IRequestHandler<GetEventByIdRequest, Event>
{
    private readonly IMongoContext _mongoContext;

    public GetEventByIdRequestHandler(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }
    
    public async Task<Event> Handle(GetEventByIdRequest request, CancellationToken cancellationToken)
    {
        var result = await _mongoContext.Events.Find(p => p.Id == request.Id).FirstOrDefaultAsync(cancellationToken);

        return result;
    }
}