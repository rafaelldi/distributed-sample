using Microsoft.EntityFrameworkCore;

namespace worker;

public class MyDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<Order> Orders { get; set; }

    public MyDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(_configuration.GetConnectionString("Postgres"));
    }
}

public record Order(Guid Id, string OrderId);