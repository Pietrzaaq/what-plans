namespace WhatPlans.Domain.Exceptions;

public class CredentialsException : WhatPlansException
{
    public CredentialsException() : base("Invalid username or password.")
    {
    }
}