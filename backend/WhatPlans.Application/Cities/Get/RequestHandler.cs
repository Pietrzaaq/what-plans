using MediatR;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Cities.Get;

public class RequestHandler : IRequestHandler<Request, List<City>>
{
    private readonly IMongoContext _mongoContext;

    public RequestHandler(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }
    
    public async Task<List<City>> Handle(Request request, CancellationToken cancellationToken)
    {
        return await _mongoContext.Cities.Find(c => true).ToListAsync(cancellationToken);
    }
}