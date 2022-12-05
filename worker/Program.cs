
using Microsoft.EntityFrameworkCore;
using worker;
using worker.Extensions;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<MyDbContext>();
        services.AddMassTransit();
        services.AddOpenTelemetry(context);
    }).ConfigureLogging((context, logging) =>
    {
        logging.AddOpenTelemetryLogging(context);
    })
    .Build();

using (var scope = host.Services.CreateScope())
{
    var myContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
    myContext.Database.Migrate();
}

await host.RunAsync();