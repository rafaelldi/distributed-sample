using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace worker.Extensions;

public static class OpenTelemetryExtensions
{
    public static IServiceCollection AddOpenTelemetry(this IServiceCollection services, HostBuilderContext context)
    {
        var resourceBuilder = ResourceBuilder.CreateDefault().AddService(context.HostingEnvironment.ApplicationName);

        services.AddOpenTelemetryTracing(it =>
        {
            it.SetResourceBuilder(resourceBuilder)
                .AddSource("MassTransit")
                .AddEntityFrameworkCoreInstrumentation()
                .AddOtlpExporter();
        });

        services.AddOpenTelemetryMetrics(it =>
        {
            it.SetResourceBuilder(resourceBuilder)
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
                .AddOtlpExporter();
        });

        return services;
    }
}