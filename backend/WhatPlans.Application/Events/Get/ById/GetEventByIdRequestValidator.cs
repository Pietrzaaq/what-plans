using FluentValidation;
using WhatPlans.Application.Interfaces;
using WhatPlans.Application.Places.Validators;

namespace WhatPlans.Application.Events.Get.ById;

public class GetEventByIdRequestValidator : AbstractValidator<GetEventByIdRequest>
{
    public GetEventByIdRequestValidator(IMongoContext context)
    {
        RuleFor(request => request.Id).NotNull();
        RuleFor(request => request.Id).SetAsyncValidator(new PlaceIdAsyncValidator<GetEventByIdRequest>(context));
    }
}