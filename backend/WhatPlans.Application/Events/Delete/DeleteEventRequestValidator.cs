using FluentValidation;
using WhatPlans.Application.Interfaces;
using WhatPlans.Application.Places.Validators;

namespace WhatPlans.Application.Events.Delete;

public class DeleteEventRequestValidator : AbstractValidator<DeleteEventRequest>
{
    public DeleteEventRequestValidator(IMongoContext context)
    {
        RuleFor(request => request.Id).NotNull();
        RuleFor(request => request.Id).SetAsyncValidator(new PlaceIdAsyncValidator<DeleteEventRequest>(context));
    }
}