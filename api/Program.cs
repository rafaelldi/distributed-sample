using contracts;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    x.UsingRabbitMq((context, cfg) => cfg.ConfigureEndpoints(context));
});

var app = builder.Build();

app.MapGet("/{orderId}", async (string orderId, IRequestClient<SubmitOrderRequest> client) =>
{
    return await client.GetResponse<SubmitOrderResponse>(new SubmitOrderRequest(orderId));
});

app.Run();