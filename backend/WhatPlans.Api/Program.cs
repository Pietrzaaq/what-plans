using AspNetCore.Identity.Mongo.Model;
using Microsoft.AspNetCore.Identity;
using WhatPlans.Api;
using WhatPlans.Application;
using WhatPlans.Infrastructure;

var allowOrigins = "AllowOrigins";

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: allowOrigins,
            policy  =>
            {
                policy.WithOrigins()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
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

    app.MapPost("/roles", async (RoleManager<MongoRole> roleManager) =>
    {
        var roles = new[] { "Standard", "Organizer", "Admin" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new MongoRole(role));
            }
        }

        return Results.Ok("Roles created successfully.");
    });

    app.Run();
}


