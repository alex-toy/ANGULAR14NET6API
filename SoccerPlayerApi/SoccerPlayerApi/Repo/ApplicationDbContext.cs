using Microsoft.EntityFrameworkCore;
using SoccerPlayerApi.Entities;
using SoccerPlayerApi.Entities.Dimensions;
using SoccerPlayerApi.Entities.Structure;

namespace SoccerPlayerApi.Repo;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<CategoryValue>().HasBaseType<DimensionValue<Category>>();
        builder.Entity<LocationValue>().HasBaseType<DimensionValue<Location>>();
        builder.Entity<TimeValue>().HasBaseType<DimensionValue<Time>>();
    }

    public DbSet<Player> Players { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CategoryValue> CategoryValues { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<LocationValue> LocationValues { get; set; }
    public DbSet<Time> Times { get; set; }
    public DbSet<TimeValue> TimeValues { get; set; }
}
