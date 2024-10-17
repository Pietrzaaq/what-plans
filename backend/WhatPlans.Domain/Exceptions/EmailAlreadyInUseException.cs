namespace WhatPlans.Domain.Exceptions;

public class EmailAlreadyInUseException : WhatPlansException
{
    public EmailAlreadyInUseException() : base("Email is already in use.")
    {
    }
}