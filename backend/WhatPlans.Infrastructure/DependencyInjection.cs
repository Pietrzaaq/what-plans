using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WhatPlans.Infrastructure.Auth;
using WhatPlans.Infrastructure.Database;

namespace WhatPlans.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        
        services.AddAuth(configuration);

        services.AddLogging();
        
        return services;
    }
}