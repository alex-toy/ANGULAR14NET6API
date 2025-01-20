using Microsoft.EntityFrameworkCore;
using SoccerPlayerApi.Entities;
using SoccerPlayerApi.Entities.Structure;

namespace SoccerPlayerApi.Repo;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AggregationFact>().ToTable("AggregationFact");

        builder.Entity<Level>()
            .HasOne(l => l.Ancestor)
            .WithMany(l => l.Children)
            .HasForeignKey(l => l.AncestorId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Level>()
            .HasOne(l => l.Dimension)
            .WithMany()
            .HasForeignKey(l => l.DimensionId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Level>()
            .HasMany(l => l.DimensionValues)
            .WithOne(l => l.Level)
            .HasForeignKey(l => l.LevelId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Dimension>()
            .HasMany(l => l.Levels)
            .WithOne(l => l.Dimension)
            .HasForeignKey(l => l.DimensionId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Entities.Environment>()
            .HasOne(e => e.LevelFilter1)
            .WithMany(l => l.Environment1s)
            .HasForeignKey(e => e.LevelIdFilter1)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Entities.Environment>()
            .HasOne(e => e.LevelFilter2)
            .WithMany(l => l.Environment2s)
            .HasForeignKey(e => e.LevelIdFilter2)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Entities.Environment>()
            .HasOne(e => e.LevelFilter3)
            .WithMany(l => l.Environment3s)
            .HasForeignKey(e => e.LevelIdFilter3)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Entities.Environment>()
            .HasOne(e => e.LevelFilter4)
            .WithMany(l => l.Environment4s)
            .HasForeignKey(e => e.LevelIdFilter4)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Entities.Environment>()
            .HasOne(e => e.LevelFilter5)
            .WithMany(l => l.Environment5s)
            .HasForeignKey(e => e.LevelIdFilter5)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Setting>()
               .HasIndex(s => s.Key)
               .IsUnique();

        builder.Entity<Setting>().HasData(
            new Setting { Id = 1, Key = "PresentDate", Value = "2024-12-01" },
            new Setting { Id = 2, Key = "PastSpan", Value = "24" },
            new Setting { Id = 3, Key = "FutureSpan", Value = "12" }
        );
    }

    public DbSet<Player> Players { get; set; }
    public DbSet<Fact> Facts { get; set; }
    public DbSet<Dimension> Dimensions { get; set; }
    public DbSet<AggregationFact> AggregationFacts { get; set; }
    public DbSet<Aggregation> Aggregations { get; set; }
    public DbSet<Level> Levels { get; set; }
    public DbSet<Entities.Environment> Environments { get; set; }
    public DbSet<Setting> Settings { get; set; }
}
