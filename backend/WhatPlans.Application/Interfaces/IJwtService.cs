using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Interfaces;

public interface IJwtService
{
    public string GenerateToken(User user);
}