using MediatR;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Places.Get.ById;

public class RequestHandler : IRequestHandler<Request, Place>
{
    private readonly IMongoContext _mongoContext;

    public RequestHandler(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }
    
    public async Task<Place> Handle(Request request, CancellationToken cancellationToken)
    {
        var place = await _mongoContext.Places.Find(p => p.Id == request.Id).FirstOrDefaultAsync(cancellationToken);

        return place;
    }
}