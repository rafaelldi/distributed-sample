using contracts;
using MassTransit;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.UsingRabbitMq((context, cfg) => cfg.ConfigureEndpoints(context));

                x.AddHandler(async (SubmitOrderRequest request) =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    return new SubmitOrderResponse($"Order {request.Id} accepted");
                });
            }
        );
    })
    .Build();

await host.RunAsync();