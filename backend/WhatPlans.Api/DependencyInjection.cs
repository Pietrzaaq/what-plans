using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using Newtonsoft.Json.Converters;
using WhatPlans.Api.Converters;

namespace WhatPlans.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration, string allowOrigins)
    {
        var origins = new List<string>();
        var origin = configuration["Cors:Origin"];

        if (string.IsNullOrWhiteSpace(origin))
            throw new Exception("Origin is not defined");
    
        origins.Add(origin);
        
        services.AddCors(options =>
        {
            options.AddPolicy(name: allowOrigins,
                policy  =>
                {
                    policy.WithOrigins(origins.ToArray())
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .AllowAnyHeader();
                });
        });
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddControllers();
        
        services.AddMvc(options =>
        {
        }).AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.Converters.Insert(0, new ObjectIdConverter());
            options.SerializerSettings.Converters.Add(new StringEnumConverter());
        });
        
        var conventionPack = new ConventionPack 
        { 
            new EnumRepresentationConvention(BsonType.String) 
        };

        ConventionRegistry.Register("EnumStringConvention", conventionPack, t => true);
        


        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.CustomSchemaIds(type => type.FullName);
        });
        
        return services;
    }
}