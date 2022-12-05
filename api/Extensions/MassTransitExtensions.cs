using MassTransit;

namespace api.Extensions;

public static class MassTransitExtensions
{
    public static WebApplicationBuilder AddMassTransit(this WebApplicationBuilder builder)
    {
        builder.Services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((context, cfg) => cfg.ConfigureEndpoints(context));
        });

        return builder;
    }
}