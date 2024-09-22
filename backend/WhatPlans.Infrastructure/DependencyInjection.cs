using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WhatPlans.Application.Interfaces;
using WhatPlans.Infrastructure.Database;

namespace WhatPlans.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetSection("Database:ConnectionString").Value;
        var database = configuration.GetSection("Database:DatabaseName").Value;
        
        var mongoDbSettings = new MongoDbSettings() { ConnectionString = connectionString, DatabaseName = database };
        services.AddSingleton(mongoDbSettings);
        services.AddScoped<IMongoContext, MongoContext>();

        return services;
    }
}