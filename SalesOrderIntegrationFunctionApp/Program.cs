using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Worker;
using SalesOrderIntegrationFunctionApp.Iservices;
using SalesOrderIntegrationFunctionApp.Services;
using Microsoft.Extensions.Azure;
using System.Security.Policy;
using Azure.Identity;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights()   
    .AddSingleton<ISecretService, SecretService>()
    .AddSingleton<IDataEncryptService, CSVEncryptService>()
    .AddSingleton<IDataConverterService, JsonToCSVService>();


builder.Services
    .AddAzureClients(clientBuilder =>
    {
        clientBuilder.AddSecretClient(new Uri("https://your-keyvault-name.vault.azure.net"))
            .WithCredential(new DefaultAzureCredential());
    });

builder.Build().Run();
