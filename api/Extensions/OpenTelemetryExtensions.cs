using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace api.Extensions;

public static class OpenTelemetryExtensions
{
    public static WebApplicationBuilder AddOpenTelemetry(this WebApplicationBuilder builder)
    {
        var resourceBuilder = ResourceBuilder.CreateDefault().AddService(builder.Environment.ApplicationName);

        builder.Services.AddOpenTelemetryTracing(it =>
        {
            it.SetResourceBuilder(resourceBuilder)
                .AddAspNetCoreInstrumentation()
                .AddSource("MassTransit")
                .AddOtlpExporter();
        });

        builder.Services.AddOpenTelemetryMetrics(it =>
        {
            it.SetResourceBuilder(resourceBuilder)
                .AddAspNetCoreInstrumentation()
                .AddRuntimeInstrumentation()
                .AddEventCountersInstrumentation(config =>
                {
                    config.AddEventSources(
                        "Microsoft.AspNetCore.Hosting",
                        "Microsoft-AspNetCore-Server-Kestrel",
                        "System.Net.Http",
                        "System.Net.Sockets",
                        "System.Net.NameResolution",
                        "System.Net.Security");
                })
                .AddMeter("MassTransit")
                .AddOtlpExporter();
        });

        builder.Logging.AddOpenTelemetry(it =>
        {
            it.SetResourceBuilder(resourceBuilder)
                .AddOtlpExporter();
        });

        return builder;
    }
}