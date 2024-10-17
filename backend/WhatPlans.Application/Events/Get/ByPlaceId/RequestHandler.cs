﻿using MediatR;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Events.Get.ByPlaceId;

public class RequestHandler : IRequestHandler<Request, List<Event>>
{
    private readonly IMongoContext _mongoContext;

    public RequestHandler(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }
    
    public async Task<List<Event>> Handle(Request request, CancellationToken cancellationToken)
    {
        var result = await _mongoContext.Events.Find(p => p.PlaceId == request.PlaceId).ToListAsync(cancellationToken);

        return result;
    }
}