using FluentValidation;
using WhatPlans.Application.Interfaces;
using WhatPlans.Application.Places.Validators;

namespace WhatPlans.Application.Places.Get.ById;

public class Validator : AbstractValidator<Request>
{
    public Validator(IMongoContext context)
    {
        RuleFor(request => request.Id).NotNull();
        RuleFor(request => request.Id).SetAsyncValidator(new PlaceIdAsyncValidator<Request>(context));
    }
}