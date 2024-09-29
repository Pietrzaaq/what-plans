using FluentValidation;
using WhatPlans.Application.Interfaces;
using WhatPlans.Application.Places.Validators;

namespace WhatPlans.Application.Places.Update;

public class UpdatePlaceRequestValidator : AbstractValidator<UpdatePlaceRequest>
{
    public UpdatePlaceRequestValidator(IMongoContext context)
    {
        RuleFor(request => request.Id).NotNull();
        RuleFor(request => request.Id).SetAsyncValidator(new PlaceIdAsyncValidator<UpdatePlaceRequest>(context));
    }
}