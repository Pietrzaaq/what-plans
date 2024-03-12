namespace WhatPlans.Domain.Exceptions;

public class WhatPlansException : Exception
{
    protected WhatPlansException(string message) : base(message)
    {
    }
}