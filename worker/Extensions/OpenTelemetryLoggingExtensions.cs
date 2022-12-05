using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

namespace worker.Extensions;

public static class OpenTelemetryLoggingExtensions
{
    public static ILoggingBuilder AddOpenTelemetryLogging(this ILoggingBuilder logging, HostBuilderContext context)
    {
        var resourceBuilder = ResourceBuilder.CreateDefault().AddService(context.HostingEnvironment.ApplicationName);

        logging.AddOpenTelemetry(it =>
        {
            it.SetResourceBuilder(resourceBuilder)
                .AddOtlpExporter();
        });

        return logging;
    }
}