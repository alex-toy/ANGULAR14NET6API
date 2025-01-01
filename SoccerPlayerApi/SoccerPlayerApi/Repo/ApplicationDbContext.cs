﻿using Microsoft.EntityFrameworkCore;
using SoccerPlayerApi.Entities;
using SoccerPlayerApi.Entities.Structure;

namespace SoccerPlayerApi.Repo;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

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
    }

    public DbSet<Player> Players { get; set; }
    public DbSet<Fact> Facts { get; set; }
    public DbSet<Dimension> Dimensions { get; set; }
    public DbSet<DimensionValue> DimensionValues { get; set; }
    public DbSet<Level> Levels { get; set; }
}
