using MassTransit.Courier.Contracts;

namespace contracts.ToDo;

public record ProcessToDoCompleted : RoutingSlipCompleted
{
    public Guid Id { get; init; }
    public Guid TrackingNumber { get; init; }
    public DateTime Timestamp { get; init; }
    public TimeSpan Duration { get; init; }
    public IDictionary<string, object>? Variables { get; init; }
}