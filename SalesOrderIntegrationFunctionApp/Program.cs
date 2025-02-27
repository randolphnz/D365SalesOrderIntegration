using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Worker;
using SalesOrderIntegrationFunctionApp.Iservices;
using SalesOrderIntegrationFunctionApp.Services;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights()   
    .AddSingleton<ISecretService, SecretService>()
    .AddSingleton<IDataEncryptService, CSVEncryptService>()
    .AddSingleton<IDataConverterService, JsonToCSVService>();


builder.Build().Run();
