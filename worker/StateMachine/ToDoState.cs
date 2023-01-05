using MassTransit;

namespace worker.StateMachine;

public class ToDoState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public required string Description { get; set; }
    public required string CurrentState { get; set; }
    public string? Result { get; set; }
}