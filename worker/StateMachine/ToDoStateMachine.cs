using contracts.ToDo;
using MassTransit;

namespace worker.StateMachine;

public class ToDoStateMachine : MassTransitStateMachine<ToDoState>
{
    public ToDoStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => ToDoItemReceived, x => x.CorrelateById(context => context.Message.Id));
        Event(() => ProcessToDoCompleted, x => x.CorrelateById(context => context.Message.Id));
        Event(() => ToDoStatusRequested, x =>
        {
            x.ReadOnly = true;
            x.CorrelateById(context => context.Message.Id);
            x.OnMissingInstance(m => m.Fault());
        });

        Initially(
            When(ToDoItemReceived)
                .Then(context =>
                {
                    LogContext.Info?.Log("Todo item received: {0}", context.Message.Id);
                    context.Saga.Description = context.Message.Description;
                })
                .Publish(context => new ProcessToDoCommand(context.Message.Id, context.Message.Description))
                .TransitionTo(Received)
        );
        During(Received,
            When(ProcessToDoCompleted)
                .Then(context =>
                {
                    var result = context.GetVariable<string>("Result");
                    LogContext.Info?.Log("Processing todo completed: {0}. Result: {1}", context.Message.Id, result);
                    context.Saga.Result = result;
                })
                .TransitionTo(Completed)
        );
        DuringAny(
            When(ToDoStatusRequested)
                .Respond(x => new GetToDoStatusResponse(
                    x.Saga.CorrelationId,
                    x.Saga.Description,
                    x.Saga.CurrentState
                ))
        );
    }

    public State? Received { get; private set; }
    public State? Completed { get; private set; }

    public Event<ToDoItemReceived>? ToDoItemReceived { get; private set; }
    public Event<ProcessToDoCompleted>? ProcessToDoCompleted { get; private set; }
    public Event<GetToDoStatusRequest>? ToDoStatusRequested { get; private set; }
}