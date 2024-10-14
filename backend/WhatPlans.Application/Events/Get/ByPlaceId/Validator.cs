using FluentValidation;
using WhatPlans.Application.Interfaces;
using WhatPlans.Application.Places.Validators;

namespace WhatPlans.Application.Events.Get.ByPlaceId;

public class Validator : AbstractValidator<Request>
{
    public Validator(IMongoContext context)
    {
        RuleFor(request => request.PlaceId).NotNull();
        RuleFor(request => request.PlaceId).SetAsyncValidator(new PlaceIdAsyncValidator<Request>(context));
    }
}