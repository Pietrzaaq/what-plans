using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WhatPlans.Application.Interfaces;
using WhatPlans.Infrastructure.Auth;
using WhatPlans.Infrastructure.Database;
using WhatPlans.Infrastructure.Services;

namespace WhatPlans.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        services.AddDatabase(configuration);
        
        services.AddAuth(configuration);

        services.AddLogging();
        
        return services;
    }
}