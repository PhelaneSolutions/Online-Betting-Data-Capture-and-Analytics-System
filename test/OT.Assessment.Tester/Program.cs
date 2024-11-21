﻿// See https://aka.ms/new-console-template for more information


using System.Net.Security;
using Microsoft.Extensions.Logging;

// Set up logging 

var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
var logger = loggerFactory.CreateLogger("NBomber");

var bg = new BogusGenerator();
var total = bg.Generate();
var scenario = Scenario.Create("hello_world_scenario", async context =>
    {
        var body = JsonSerializer.Serialize(total[(int)context.InvocationNumber]);
        using var httpClient = new HttpClient();
        var request =
            Http.CreateRequest("POST", "http://localhost:5021/api/player/casinoWager")
                .WithHeader("Accept", "application/json")
                .WithBody(new StringContent($"{body}", Encoding.UTF8, "application/json"));

        var response = await Http.Send(httpClient, request);

        if (response.StatusCode == "Ok") 
        { 
            logger.LogInformation($"Request succeeded: {response.StatusCode}"); 
            return Response.Ok(); 
        }
        return Response.Fail(body, response.StatusCode, response.Message, response.SizeBytes);
  
    })
    .WithoutWarmUp()
    .WithLoadSimulations(
        Simulation.IterationsForInject(rate: 500,
            interval: TimeSpan.FromSeconds(2),
            iterations: 7000)
    );

NBomberRunner
    .RegisterScenarios(scenario)
    .WithWorkerPlugins(new HttpMetricsPlugin(new[] { HttpVersion.Version1 }))
    .WithoutReports()
    .Run();