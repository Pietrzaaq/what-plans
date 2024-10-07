using FluentValidation;
using WhatPlans.Application.Interfaces;
using WhatPlans.Application.Places.Validators;

namespace WhatPlans.Application.Events.Get.ByPlaceId;

public class GetEventsByPlaceIdRequestValidator : AbstractValidator<GetEventsByPlaceIdRequest>
{
    public GetEventsByPlaceIdRequestValidator(IMongoContext context)
    {
        RuleFor(request => request.PlaceId).NotNull();
        RuleFor(request => request.PlaceId).SetAsyncValidator(new PlaceIdAsyncValidator<GetEventsByPlaceIdRequest>(context));
    }
}