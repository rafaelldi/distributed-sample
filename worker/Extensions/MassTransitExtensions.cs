using contracts;
using MassTransit;

namespace worker.Extensions;

public static class MassTransitExtensions
{
    public static IServiceCollection AddMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.UsingRabbitMq((context, cfg) => cfg.ConfigureEndpoints(context));

                x.AddHandler(async (SubmitOrderRequest request, MyDbContext context) =>
                {
                    context.Orders.Add(new Order(Guid.NewGuid(), request.Id));
                    await context.SaveChangesAsync();

                    return new SubmitOrderResponse($"Order {request.Id} accepted");
                });
            }
        );

        return services;
    }
}