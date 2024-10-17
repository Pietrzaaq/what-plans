using Microsoft.AspNetCore.Identity;

namespace WhatPlans.Domain.Exceptions;

public class IdentityException : WhatPlansException
{
    public IEnumerable<IdentityError> Errors { get; }
    
    public IdentityException(IEnumerable<IdentityError> errors) : base("Identity exception occured")
    {
        Errors = errors;
    }
}