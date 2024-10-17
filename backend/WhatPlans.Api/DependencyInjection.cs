using WhatPlans.Api.Converters;

namespace WhatPlans.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddControllers();
        
        services.AddMvc(options =>
        {
        }).AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.Converters.Insert(0, new ObjectIdConverter());
        });
        
        return services;
    }
}