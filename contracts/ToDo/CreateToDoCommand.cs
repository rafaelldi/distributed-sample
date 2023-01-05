namespace contracts.ToDo;

public record CreateToDoCommand(Guid Id, string Description);