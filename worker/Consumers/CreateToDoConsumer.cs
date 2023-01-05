using contracts.ToDo;
using MassTransit;

namespace worker.Consumers;

public class CreateToDoConsumer : IConsumer<CreateToDoCommand>
{
    private readonly ILogger<CreateToDoConsumer> _logger;

    public CreateToDoConsumer(ILogger<CreateToDoConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CreateToDoCommand> context)
    {
        var command = context.Message;

        _logger.LogInformation($"Consumed command to create todo: {command.Id}, {command.Description}");

        await context.Publish(new ToDoItemReceived(command.Id, command.Description));
    }
}