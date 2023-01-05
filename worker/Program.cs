
using Microsoft.EntityFrameworkCore;
using worker.DbContexts;
using worker.Extensions;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<PingPongDbContext>();
        services.AddDbContext<ToDoStateDbContext>();
        services.AddMassTransit();
        services.AddOpenTelemetry(context);
    }).ConfigureLogging((context, logging) =>
    {
        logging.AddOpenTelemetryLogging(context);
    })
    .Build();

using (var scope = host.Services.CreateScope())
{
    var pingPongDbContext = scope.ServiceProvider.GetRequiredService<PingPongDbContext>();
    pingPongDbContext.Database.Migrate();
    var toDoStateDbContext = scope.ServiceProvider.GetRequiredService<ToDoStateDbContext>();
    toDoStateDbContext.Database.Migrate();
}

await host.RunAsync();