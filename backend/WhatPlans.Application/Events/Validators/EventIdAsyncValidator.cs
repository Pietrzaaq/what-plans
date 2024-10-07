using FluentValidation;
using FluentValidation.Validators;
using MongoDB.Bson;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;

namespace WhatPlans.Application.Events.Validators;

public class EventIdAsyncValidator<TRequest> : AsyncPropertyValidator<TRequest, ObjectId>
{
    private readonly IMongoContext _mongoContext;

    public EventIdAsyncValidator(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }

    public override async Task<bool> IsValidAsync(ValidationContext<TRequest> context, ObjectId value, CancellationToken cancellation)
    {
        return await _mongoContext.Events.Find(p => p.Id == value).AnyAsync(cancellationToken: cancellation);
    }
    
    public override string Name => GetType().Name;
    
    protected override string GetDefaultMessageTemplate(string errorCode)
        => "Event with id: {PropertyValue} does not exist.";
}
