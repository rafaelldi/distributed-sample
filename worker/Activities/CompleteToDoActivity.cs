using MassTransit;

namespace worker.Activities;

public class CompleteToDoActivity : IExecuteActivity<CompleteToDoArguments>
{
    private readonly ILogger<CompleteToDoActivity> _logger;

    public CompleteToDoActivity(ILogger<CompleteToDoActivity> logger)
    {
        _logger = logger;
    }

    public async Task<ExecutionResult> Execute(ExecuteContext<CompleteToDoArguments> context)
    {
        var arguments = context.Arguments;

        _logger.LogInformation($"Processing todo with description: {arguments.Description}");

        await Task.Delay(TimeSpan.FromSeconds(10));
        
        return context.CompletedWithVariables(new { Result = "OK" });
    }
}