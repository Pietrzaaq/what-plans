using WhatPlans.Application.Interfaces;

namespace WhatPlans.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}