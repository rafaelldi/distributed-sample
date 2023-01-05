using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace worker.DbContexts;

public class ToDoStateDbContext : SagaDbContext
{
    private readonly IConfiguration _configuration;

    public ToDoStateDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(_configuration.GetConnectionString("Sagas"));
    }

    protected override IEnumerable<ISagaClassMap> Configurations => new[] { new ToDoStateMap() };
}