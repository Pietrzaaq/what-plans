using FluentValidation;
using WhatPlans.Application.Interfaces;
using WhatPlans.Application.Places.Validators;

namespace WhatPlans.Application.Places.Delete;

public class DeletePlaceRequestValidator : AbstractValidator<DeletePlaceRequest>
{
    public DeletePlaceRequestValidator(IMongoContext context)
    {
        RuleFor(request => request.Id).NotNull();
        RuleFor(request => request.Id).SetAsyncValidator(new PlaceIdAsyncValidator<DeletePlaceRequest>(context));
    }
}