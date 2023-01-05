namespace contracts.ToDo;

public record GetToDoStatusResponse(Guid Id, string Description, string Status);