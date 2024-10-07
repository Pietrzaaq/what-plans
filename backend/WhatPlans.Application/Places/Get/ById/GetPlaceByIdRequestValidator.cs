using FluentValidation;
using WhatPlans.Application.Interfaces;
using WhatPlans.Application.Places.Validators;

namespace WhatPlans.Application.Places.Get.ById;

public class GetPlaceByIdRequestValidator : AbstractValidator<GetPlaceByIdRequest>
{
    public GetPlaceByIdRequestValidator(IMongoContext context)
    {
        RuleFor(request => request.Id).NotNull();
        RuleFor(request => request.Id).SetAsyncValidator(new PlaceIdAsyncValidator<GetPlaceByIdRequest>(context));
    }
}