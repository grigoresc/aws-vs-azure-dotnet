using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);


// Application Insights isn't enabled ();
//     .AddApplicationInsightsTelemetryWorkerService()

builder.Build().Run();
