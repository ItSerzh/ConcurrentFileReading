using ConcurrentFileReading;
using ConcurrentFileReading.Implementations;
using ConcurrentFileReading.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var cfg = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<IConfiguration>(cfg);
builder.Services.AddSingleton<IDiskProcessor, DiskProcessor>();
builder.Services.AddSingleton<IOutput, ConsoleOutput>();
builder.Services.AddSingleton<TaskRunner>();

using IHost host = builder.Build();
var scope = host.Services.CreateScope();

var taskRunner = scope.ServiceProvider.GetRequiredService<TaskRunner>();

await taskRunner.StartInvestigation();

