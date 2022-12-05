using api.Extensions;
using contracts;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddMassTransit()
    .AddOpenTelemetry();

var app = builder.Build();

app.MapGet("/{orderId}", async (string orderId, IRequestClient<SubmitOrderRequest> client) =>
{
    return await client.GetResponse<SubmitOrderResponse>(new SubmitOrderRequest(orderId));
});

app.Run();