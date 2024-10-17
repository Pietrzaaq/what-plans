using FluentValidation;
using FluentValidation.Validators;
using MongoDB.Bson;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;

namespace WhatPlans.Application.Users.Validators;

public class UserIdAsyncValidator<TRequest> : AsyncPropertyValidator<TRequest, ObjectId>
{
    private readonly IMongoContext _mongoContext;

    public UserIdAsyncValidator(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }

    public override async Task<bool> IsValidAsync(ValidationContext<TRequest> context, ObjectId value, CancellationToken cancellation)
    {
        return await _mongoContext.Users.Find(p => p.Id == value).AnyAsync(cancellationToken: cancellation);
    }
    
    public override string Name => GetType().Name;
    
    protected override string GetDefaultMessageTemplate(string errorCode)
        => "User with id: {PropertyValue} does not exist.";
}
