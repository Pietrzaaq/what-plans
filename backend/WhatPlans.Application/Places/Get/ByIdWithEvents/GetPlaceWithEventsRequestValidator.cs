using FluentValidation;
using WhatPlans.Application.Interfaces;
using WhatPlans.Application.Places.Validators;

namespace WhatPlans.Application.Places.Get.ByIdWithEvents;

public class GetPlaceWithEventsRequestValidator : AbstractValidator<GetPlaceWithEventsRequest>
{
    public GetPlaceWithEventsRequestValidator(IMongoContext context)
    {
        RuleFor(request => request.Id).NotNull();
        RuleFor(request => request.Id).SetAsyncValidator(new PlaceIdAsyncValidator<GetPlaceWithEventsRequest>(context));
    }
}