using contracts.ToDo;
using MassTransit;
using MassTransit.Courier.Contracts;
using worker.Activities;

namespace worker.Consumers;

public class ProcessToDoConsumer : IConsumer<ProcessToDoCommand>
{
    private readonly ILogger<ProcessToDoConsumer> _logger;
    private readonly IEndpointNameFormatter _endpointNameFormatter;

    public ProcessToDoConsumer(ILogger<ProcessToDoConsumer> logger, IEndpointNameFormatter endpointNameFormatter)
    {
        _logger = logger;
        _endpointNameFormatter = endpointNameFormatter;
    }

    public async Task Consume(ConsumeContext<ProcessToDoCommand> context)
    {
        var command = context.Message;

        _logger.LogInformation($"Consumed command to process todo: {command.Id}, {command.Description}");

        var routingSlip = CreateRoutingSlip(command, context.SourceAddress);
        await context.Execute(routingSlip);
    }

    private RoutingSlip CreateRoutingSlip(ProcessToDoCommand command, Uri? sourceAddress)
    {
        var builder = new RoutingSlipBuilder(NewId.NextGuid());

        builder.AddActivity("CompleteToDo", GetActivityAddress<CompleteToDoActivity, CompleteToDoArguments>(),
            new { command.Description });

        if (sourceAddress != null)
        {
            builder.AddSubscription(sourceAddress, RoutingSlipEvents.Completed,
                x => x.Send<ProcessToDoCompleted>(new { command.Id }));
        }
        
        return builder.Build();
    }
    
    private Uri GetActivityAddress<TActivity, TArguments>()
        where TActivity : class, IExecuteActivity<TArguments>
        where TArguments : class
    {
        var name = _endpointNameFormatter.ExecuteActivity<TActivity, TArguments>();
        return new Uri($"exchange:{name}");
    }
}