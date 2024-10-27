using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;
using WhatPlans.Domain.Enums;

namespace WhatPlans.Infrastructure.Auth;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _services;

    public JwtService(IConfiguration configuration, IServiceProvider services)
    {
        _configuration = configuration;
        _services = services;
    }

    public string GenerateToken(User user)
    {
        var jwtSettings = _configuration.GetSection("Authorization");
        
        if (!jwtSettings.Exists())
            throw new ApplicationException("Authorization configuration section doesn't exist.");

        var key = jwtSettings["IssuerSigningKey"];
        if (key == null)
            throw new ApplicationException("Secret key configuration section doesn't exist.");

        using var scope = _services.CreateScope();
        var dateTimeProvider = scope.ServiceProvider.GetRequiredService<IDateTimeProvider>();
            
        var secret = Encoding.UTF8.GetBytes(key);
        var signingCredentials =
            new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var now = dateTimeProvider.UtcNow;
        var expires= now.AddDays(7);

        UserAccount role;
        if (user.IsAdmin)
            role = UserAccount.Admin;
        else if (user.IsOrganizer)
            role = UserAccount.Organizer;
        else
            role = UserAccount.Standard;

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.Id.ToString()),
            new(ClaimTypes.Role, role.ToString()),
        };

        var jwt = new JwtSecurityToken(issuer, audience, claims, now, expires, signingCredentials);
        var tokenHandler = new JwtSecurityTokenHandler();

        var accessToken = tokenHandler.WriteToken(jwt);
        return accessToken;
    }
}