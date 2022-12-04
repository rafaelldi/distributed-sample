using contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using worker;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDbContext<MyDbContext>();
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
    })
    .Build();

using (var scope = host.Services.CreateScope())
{
    var myContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
    myContext.Database.Migrate();
}

await host.RunAsync();