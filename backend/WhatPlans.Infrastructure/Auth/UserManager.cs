using Microsoft.AspNetCore.Identity;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Infrastructure.Auth;

public class UserManager : IUserManager
{
    private readonly UserManager<User> _userManager;

    public UserManager(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<User> FindByUsernameAsync(string username)
    {
        return await _userManager.FindByNameAsync(username);
    }
    
    public async Task<User> FindByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<bool> CheckPasswordAsync(User user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<IdentityResult> Register(User user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }
}