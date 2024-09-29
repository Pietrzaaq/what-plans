using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace WhatPlans.Application;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseRequest
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var errors = new List<ValidationFailure>();

        foreach (var validator in _validators)
        {
            var validationResult = await validator.ValidateAsync(context, cancellationToken);

            if (!validationResult.IsValid)
                errors.AddRange(validationResult.Errors);
        }

        if (errors.Count != 0)
            throw new ValidationException(errors);

        return await next();
    }
}