using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Infrastructure.Auth;

public class CurrentUserProvider : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMongoContext _mongoContext;

    public CurrentUserProvider(IHttpContextAccessor httpContextAccessor, IMongoContext mongoContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _mongoContext = mongoContext;
    }

    public async Task<User> GetUser()
    {
        if (_httpContextAccessor.HttpContext?.User.Identity == null)
            return null;
            
        var userId = ObjectId.Parse(_httpContextAccessor?.HttpContext.User.Identity.Name);
        var user = await _mongoContext.Users.Find(u => u.Id == userId).FirstOrDefaultAsync();
        return user;
    }
}