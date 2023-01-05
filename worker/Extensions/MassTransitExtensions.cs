using contracts;
using contracts.PingPong;
using MassTransit;
using worker.Activities;
using worker.Consumers;
using worker.DbContexts;
using worker.StateMachine;

namespace worker.Extensions;

public static class MassTransitExtensions
{
    public static IServiceCollection AddMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.UsingRabbitMq((context, cfg) => cfg.ConfigureEndpoints(context));

                x.AddHandler(async (PingPongRequest request, PingPongDbContext context) =>
                {
                    context.PingPongModels.Add(new PingPongModel(Guid.NewGuid(), request.Value));
                    await context.SaveChangesAsync();

                    return new PingPongResponse($"{request.Value}-pong");
                });

                x.AddConsumer<CreateToDoConsumer>();
                x.AddConsumer<ProcessToDoConsumer>();
                x.AddExecuteActivity<CompleteToDoActivity, CompleteToDoArguments>();

                x.AddSagaStateMachine<ToDoStateMachine, ToDoState>();

                x.SetEntityFrameworkSagaRepositoryProvider(r =>
                {
                    r.ExistingDbContext<ToDoStateDbContext>();
                    r.UsePostgres();
                });
            }
        );

        return services;
    }
}