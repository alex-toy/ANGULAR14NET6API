﻿using Microsoft.EntityFrameworkCore;
using SoccerPlayerApi.Entities;
using SoccerPlayerApi.Entities.Environments;
using SoccerPlayerApi.Entities.Structure;
using SoccerPlayerApi.Utils;

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

        ConfigureLevel(builder);

        builder.Entity<Dimension>()
            .HasMany(l => l.Levels)
            .WithOne(l => l.Dimension)
            .HasForeignKey(l => l.DimensionId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Aggregation>()
            .HasOne(l => l.MotherAggregation)
            .WithMany()
            .HasForeignKey(l => l.MotherAggregationId)
            .OnDelete(DeleteBehavior.NoAction);

        ConfigureEnvironment(builder);

        ConfigureEnvironmentScope(builder);

        builder.Entity<Setting>()
               .HasIndex(s => s.Key)
               .IsUnique();

        SeedData(builder);
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
    public DbSet<EnvironmentScope> EnvironmentScopes { get; set; }
    public DbSet<EnvironmentSorting> EnvironmentSortings { get; set; }

    private static void ConfigureLevel(ModelBuilder builder)
    {
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
    }

    private static void ConfigureEnvironment(ModelBuilder builder)
    {
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

        builder.Entity<EnvironmentSorting>()
            .HasOne(e => e.Environment)
            .WithMany()
            .HasForeignKey(e => e.EnvironmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<EnvironmentSorting>()
            .HasOne(e => e.DataType)
            .WithMany()
            .HasForeignKey(e => e.DataTypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureEnvironmentScope(ModelBuilder builder)
    {
        builder.Entity<EnvironmentScope>()
            .HasOne(e => e.Dimension1Aggregation)
            .WithMany()
            .HasForeignKey(e => e.Dimension1AggregationId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<EnvironmentScope>()
            .HasOne(e => e.Dimension2Aggregation)
            .WithMany()
            .HasForeignKey(e => e.Dimension2AggregationId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<EnvironmentScope>()
            .HasOne(e => e.Dimension3Aggregation)
            .WithMany()
            .HasForeignKey(e => e.Dimension3AggregationId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<EnvironmentScope>()
            .HasOne(e => e.Dimension4Aggregation)
            .WithMany()
            .HasForeignKey(e => e.Dimension4AggregationId)
            .OnDelete(DeleteBehavior.NoAction);
    }

    private static void SeedData(ModelBuilder builder)
    {
        builder.Entity<Setting>().HasData(
            new Setting { Id = 1, Key = "PresentWeekDate", Value = "2024-W09" },
            new Setting { Id = 2, Key = "PastWeekSpan", Value = "10" },
            new Setting { Id = 3, Key = "PresentMonthDate", Value = "2024-M09" },
            new Setting { Id = 4, Key = "PastMonthSpan", Value = "10" }
        );

        builder.Entity<Dimension>().HasData(
            new Dimension { Id = 1, Value = "Time" }
        );

        builder.Entity<Level>().HasData(
            new Level { Id = GlobalVar.YEAR_LEVEL_ID, DimensionId = GlobalVar.TIME_DIMENSION_ID, Value = "YEAR" },
            new Level { Id = GlobalVar.SEMESTER_LEVEL_ID, DimensionId = GlobalVar.TIME_DIMENSION_ID, Value = "SEMESTER", AncestorId = GlobalVar.YEAR_LEVEL_ID },
            new Level { Id = GlobalVar.TRIMESTER_LEVEL_ID, DimensionId = GlobalVar.TIME_DIMENSION_ID, Value = "TRIMESTER", AncestorId = GlobalVar.SEMESTER_LEVEL_ID },
            new Level { Id = GlobalVar.MONTH_LEVEL_ID, DimensionId = GlobalVar.TIME_DIMENSION_ID, Value = "MONTH", AncestorId = GlobalVar.TRIMESTER_LEVEL_ID },
            new Level { Id = GlobalVar.WEEK_LEVEL_ID, DimensionId = GlobalVar.TIME_DIMENSION_ID, Value = "WEEK", AncestorId = GlobalVar.MONTH_LEVEL_ID }
        );
    }
}
