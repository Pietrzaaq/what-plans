namespace WhatPlans.Domain.Exceptions;

public class UsernameAlreadyInUseException : WhatPlansException
{
    public UsernameAlreadyInUseException() : base("Email is already in use.")
    {
    }
}