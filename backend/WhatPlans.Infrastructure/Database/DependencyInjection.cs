using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WhatPlans.Application.Interfaces;

namespace WhatPlans.Infrastructure.Database;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetSection("Database:ConnectionString").Value;
        var database = configuration.GetSection("Database:DatabaseName").Value;
        
        var mongoDbSettings = new MongoDbSettings() { ConnectionString = connectionString, DatabaseName = database };
        services.AddSingleton(mongoDbSettings);
        services.AddScoped<IMongoContext, MongoContext>();
        
        return services;
    }
}