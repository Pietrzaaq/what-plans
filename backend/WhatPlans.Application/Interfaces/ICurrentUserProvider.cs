using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Interfaces;

public interface ICurrentUserProvider
{
    public Task<User> GetUser();
}