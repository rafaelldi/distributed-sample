using Microsoft.EntityFrameworkCore;

namespace worker.DbContexts;

public class PingPongDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<PingPongModel> PingPongModels { get; set; }

    public PingPongDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(_configuration.GetConnectionString("PingPong"));
    }
}

public record PingPongModel(Guid Id, string Value);