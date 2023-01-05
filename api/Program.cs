using api;
using api.Extensions;
using contracts.PingPong;
using contracts.ToDo;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddMassTransit()
    .AddOpenTelemetry();

var app = builder.Build();

app.MapPost("/ping-pong", async (IRequestClient<PingPongRequest> client) =>
{
    var response = await client.GetResponse<PingPongResponse>(new PingPongRequest("ping"));
    return Results.Ok(response.Message);
});

app.MapPost("/todo", async (ToDo toDo, IPublishEndpoint publishEndpoint) =>
{
    await publishEndpoint.Publish(new CreateToDoCommand(toDo.Id, toDo.Description));
    return Results.Created($"/todo/{toDo.Id}", toDo);
});

app.MapGet("/todo/{id:guid}", async (Guid id, IRequestClient<GetToDoStatusRequest> client) =>
{
    try
    {
        var response = await client.GetResponse<GetToDoStatusResponse>(new GetToDoStatusRequest(id));
        return Results.Ok(response.Message);
    }
    catch (RequestFaultException ex)
    {
        return Results.NotFound(ex.Message);
    }
});

app.Run();