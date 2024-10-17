using Microsoft.AspNetCore.Identity;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Interfaces;

public interface IUserManager
{
    public Task<User> FindByUsernameAsync(string username);
    public Task<User> FindByEmailAsync(string email);
    public Task<bool> CheckPasswordAsync(User user, string password);
    public Task<IdentityResult> Register(User user, string password);
}