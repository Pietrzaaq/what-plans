using FluentValidation;
using WhatPlans.Application.Interfaces;
using WhatPlans.Application.Users.Validators;

namespace WhatPlans.Application.Users.Get.ById;

public class Validator : AbstractValidator<Request>
{
    public Validator(IMongoContext context)
    {
        RuleFor(request => request.Id).NotNull();
        RuleFor(request => request.Id).SetAsyncValidator(new UserIdAsyncValidator<Request>(context));
    }
}