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
    builder.Services.AddSwaggerGen();

    builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration)
        .AddPresentation();
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.MapControllers();
    app.UseHttpsRedirection();
    app.UseCors(allowOrigins);

    app.Run();
}


