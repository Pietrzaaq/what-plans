namespace WhatPlans.Application.Interfaces;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}