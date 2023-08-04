using Elastic.Apm.AspNetCore;
using Elastic.Apm.NetCoreAll;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
        tracerProviderBuilder
            .AddSource(DiagnosticsConfig.ActivitySource.Name)
            .ConfigureResource(resource => resource
                .AddService(DiagnosticsConfig.ServiceName))
            .AddAspNetCoreInstrumentation()
            .AddConsoleExporter());


var app = builder.Build();

// Configure the HTTP request pipeline.

Environment.SetEnvironmentVariable("OTEL_EXPORTER_OLTP_ENDPOINT", "https://4760a6ce142b46fb9accf37b43fab918.apm.us-central1.gcp.cloud.es.io:443");
Environment.SetEnvironmentVariable("OTEL_EXPORTER_OLTP_HEADERS", "Authorization=Bearer auBpHbjNHshXDDMd1O");
Environment.SetEnvironmentVariable("OTEL_METRICS_EXPORTER", "otlp");
Environment.SetEnvironmentVariable("OTEL_LOGS_EXPORTER", "otlp");
Environment.SetEnvironmentVariable("OTEL_RESOURCE_ATTRIBUTES", "service.name=dotNet,service.version=1.0,deployment.environment=production");
Environment.SetEnvironmentVariable("OTEL_DOTNET_AUTO_HOME", "/otel");

app.UseAllElasticApm(app.Configuration);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// public static class DiagnosticsConfig
// {
//     public const string ServiceName = "MyService";
//     public static ActivitySource ActivitySource = new ActivitySource(ServiceName);
// }
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
            webBuilder.UseUrls("http://*:5000"); // Change the port to 5000
        });

