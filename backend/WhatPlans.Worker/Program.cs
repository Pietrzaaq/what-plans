using WhatPlans.Application.Interfaces;
using WhatPlans.Infrastructure.Database;
using WhatPlans.Worker;
using WhatPlans.Worker.Models;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var connectionString = builder.Configuration.GetSection("Database:ConnectionString").Value;
var database = builder.Configuration.GetSection("Database:DatabaseName").Value;
        
var mongoDbSettings = new MongoDbSettings() { ConnectionString = connectionString, DatabaseName = database };
builder.Services.AddSingleton(mongoDbSettings);
builder.Services.AddSingleton<IMongoContext, MongoContext>();

builder.Services.Configure<TicketMasterSettings>(builder.Configuration.GetSection("TicketMasterSettings"));
builder.Services.AddHttpClient();

var host = builder.Build();
host.Run();