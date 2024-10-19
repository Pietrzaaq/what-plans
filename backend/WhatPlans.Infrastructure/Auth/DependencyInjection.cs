using System.Text;
using AspNetCore.Identity.Mongo;
using AspNetCore.Identity.Mongo.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Infrastructure.Auth;

public static class DependencyInjection
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetSection("Database:ConnectionString").Value;
        var database = configuration.GetSection("Database:DatabaseName").Value;
        
        services.AddScoped<IUserManager, UserManager>();
        services.AddSingleton<IJwtService, JwtService>();
        services.AddIdentityMongoDbProvider<User, MongoRole>(identityOptions =>
            {
                identityOptions.Password.RequireDigit = false;
                identityOptions.Password.RequiredLength = 6;
                identityOptions.Password.RequireLowercase = false;
            },
            mongoIdentityOptions =>
            {
                mongoIdentityOptions.ConnectionString = $"{connectionString}/{database}/?authSource={database}";
            });
        
        var issuer = configuration.GetSection("Authorization:Issuer").Value;
        var audience = configuration.GetSection("Authorization:Audience").Value;
        var key = configuration.GetSection("Authorization:IssuerSigningKey").Value;
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.IncludeErrorDetails = true;
                options.Audience = audience;
                
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = issuer,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
            });

        services.AddAuthorization();
        
        return services;
    }
}