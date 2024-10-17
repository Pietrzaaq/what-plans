using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WhatPlans.Infrastructure.Logging.Decorators;

namespace WhatPlans.Infrastructure.Logging;

public static class DependencyInjection
{
    public static IServiceCollection AddLogging(this IServiceCollection services, IConfiguration configuration)
    {
        services.TryDecorate(typeof(IRequestHandler<>), typeof(LoggingRequestHandlerDecorator<>));
            
        return services;
    }
}