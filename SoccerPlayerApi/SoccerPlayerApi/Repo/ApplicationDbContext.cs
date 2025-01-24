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
        builder.Entity<TimeDimension>().ToTable("DateSeries");

        builder.Entity<Fact>()
            .HasOne(e => e.DataType)
            .WithMany(l => l.Facts)
            .HasForeignKey(e => e.DataTypeId)
            .OnDelete(DeleteBehavior.NoAction);

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

        builder.Entity<Dimension>().HasData(
            new Dimension { Id = 1, Value = "Time" }
        );

        builder.Entity<Level>().HasData(
            new Level { Id = 1, DimensionId = 1, Value = "YEAR" },
            new Level { Id = 2, DimensionId = 1, Value = "QUARTER" },
            new Level { Id = 3, DimensionId = 1, Value = "TRIMESTER" },
            new Level { Id = 4, DimensionId = 1, Value = "MONTH" },
            new Level { Id = 5, DimensionId = 1, Value = "WEEK" }
        );
    }

    public DbSet<TimeDimension> DateSeries { get; set; }
    public DbSet<Fact> Facts { get; set; }
    public DbSet<DataType> DataTypes { get; set; }
    public DbSet<Dimension> Dimensions { get; set; }
    public DbSet<AggregationFact> AggregationFacts { get; set; }
    public DbSet<Aggregation> Aggregations { get; set; }
    public DbSet<Level> Levels { get; set; }
    public DbSet<Entities.Environment> Environments { get; set; }
    public DbSet<Setting> Settings { get; set; }
}
