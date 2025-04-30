using WhatPlans.Job;
using WhatPlans.Job.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<CitiesService>();
builder.Services.AddSingleton<GeoJsonService>();
builder.Services.AddSingleton<MongoService>();
builder.Services.AddSingleton<ImageService>();

var host = builder.Build();
host.Run();