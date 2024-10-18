using AspNetCore.Identity.Mongo.Model;
using Microsoft.AspNetCore.Identity;
using WhatPlans.Api;
using WhatPlans.Application;
using WhatPlans.Infrastructure;

var allowOrigins = "AllowOrigins";

var builder = WebApplication.CreateBuilder(args);
{
    var origins = new List<string>();
    var origin = builder.Configuration["Cors:Origin"];

    if (string.IsNullOrWhiteSpace(origin))
        throw new Exception("Origin is not defined");
    
    origins.Add(origin);
    
    builder.Services.AddCors(options =>
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

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.CustomSchemaIds(type => type.FullName);
    });

    builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration)
        .AddPresentation(builder.Configuration);
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseHttpsRedirection();
    app.UseCors(allowOrigins);
    app.MapControllers();

    app.Run();
}


