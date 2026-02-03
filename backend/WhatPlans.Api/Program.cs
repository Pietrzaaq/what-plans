using Serilog;
using WhatPlans.Api;
using WhatPlans.Application;
using WhatPlans.Infrastructure;

var allowOrigins = "AllowOrigins";

var builder = WebApplication.CreateBuilder(args);
{
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console()
        .CreateLogger();
    
    builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration)
        .AddPresentation(builder.Configuration, allowOrigins);
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


