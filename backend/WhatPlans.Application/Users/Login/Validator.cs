using FluentValidation;

namespace WhatPlans.Application.Users.Login;

public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(request => request.Body)
            .NotNull();

        When(request => request.Body is not null, () =>
        {
            RuleFor(r => r.Body.Email)
                .NotNull()
                .EmailAddress();
            
            RuleFor(r => r.Body.Password)
                .NotNull()
                .NotEmpty();
        });
    }
}