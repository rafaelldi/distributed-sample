using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using worker.StateMachine;

namespace worker.DbContexts;

public class ToDoStateMap : SagaClassMap<ToDoState>
{
    protected override void Configure(EntityTypeBuilder<ToDoState> entity, ModelBuilder model)
    {
        entity.Property(x => x.Description);
        entity.Property(x => x.CurrentState);
        entity.Property(x => x.Result);
    }
}