using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OT.Assessment.Consumer.Db;
using OT.Assessment.Consumer.Interfaces;
using OT.Assessment.Consumer.Messaging;
using OT.Assessment.Consumer.Service;

var builder =Host.CreateDefaultBuilder(args);

builder.ConfigureServices((hostCntext, services) =>{
    services.AddDbContext<ConsumerDb>(options => 
        options.UseSqlServer(hostCntext.Configuration.GetConnectionString("DatabaseConnection")));
    services.AddSingleton<IRabbitMqClient, RabbitMqClient>();
    services.AddHostedService<ConsumeService>();
    services.AddMemoryCache();
    services.AddLogging(loggingBuilder => 
    {
        loggingBuilder.AddConsole();
        loggingBuilder.AddDebug();
    });
});

await builder.Build().RunAsync();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config =>
    {
        config.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
    })
    .ConfigureServices((context, services) =>
    {
        //configure services
      
    })
    .Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Application started {time:yyyy-MM-dd HH:mm:ss}", DateTime.Now);

await host.RunAsync();

logger.LogInformation("Application ended {time:yyyy-MM-dd HH:mm:ss}", DateTime.Now);