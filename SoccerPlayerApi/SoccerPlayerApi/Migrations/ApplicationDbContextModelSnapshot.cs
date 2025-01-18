﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SoccerPlayerApi.Repo;

#nullable disable

namespace SoccerPlayerApi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.36")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SoccerPlayerApi.Entities.Environment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LevelIdFilter1")
                        .HasColumnType("int");

                    b.Property<int?>("LevelIdFilter2")
                        .HasColumnType("int");

                    b.Property<int?>("LevelIdFilter3")
                        .HasColumnType("int");

                    b.Property<int?>("LevelIdFilter4")
                        .HasColumnType("int");

                    b.Property<int?>("LevelIdFilter5")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LevelIdFilter1");

                    b.HasIndex("LevelIdFilter2");

                    b.HasIndex("LevelIdFilter3");

                    b.HasIndex("LevelIdFilter4");

                    b.HasIndex("LevelIdFilter5");

                    b.ToTable("Environments");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("JerseyNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.Aggregation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("LevelId")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LevelId");

                    b.ToTable("Aggregations");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.Dimension", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Dimensions");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.DimensionFact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("DimensionValueId")
                        .HasColumnType("int");

                    b.Property<int>("FactId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DimensionValueId");

                    b.HasIndex("FactId");

                    b.ToTable("DimensionFact", (string)null);
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.Fact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<decimal>("Amount")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Facts");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.Level", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("AncestorId")
                        .HasColumnType("int");

                    b.Property<int>("DimensionId")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AncestorId");

                    b.HasIndex("DimensionId");

                    b.ToTable("Levels");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Environment", b =>
                {
                    b.HasOne("SoccerPlayerApi.Entities.Structure.Level", "LevelFilter1")
                        .WithMany("Environment1s")
                        .HasForeignKey("LevelIdFilter1")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SoccerPlayerApi.Entities.Structure.Level", "LevelFilter2")
                        .WithMany("Environment2s")
                        .HasForeignKey("LevelIdFilter2")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("SoccerPlayerApi.Entities.Structure.Level", "LevelFilter3")
                        .WithMany("Environment3s")
                        .HasForeignKey("LevelIdFilter3")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("SoccerPlayerApi.Entities.Structure.Level", "LevelFilter4")
                        .WithMany("Environment4s")
                        .HasForeignKey("LevelIdFilter4")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("SoccerPlayerApi.Entities.Structure.Level", "LevelFilter5")
                        .WithMany("Environment5s")
                        .HasForeignKey("LevelIdFilter5")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("LevelFilter1");

                    b.Navigation("LevelFilter2");

                    b.Navigation("LevelFilter3");

                    b.Navigation("LevelFilter4");

                    b.Navigation("LevelFilter5");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.Aggregation", b =>
                {
                    b.HasOne("SoccerPlayerApi.Entities.Structure.Level", "Level")
                        .WithMany("DimensionValues")
                        .HasForeignKey("LevelId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Level");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.DimensionFact", b =>
                {
                    b.HasOne("SoccerPlayerApi.Entities.Structure.Aggregation", "DimensionValue")
                        .WithMany()
                        .HasForeignKey("DimensionValueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SoccerPlayerApi.Entities.Structure.Fact", "Fact")
                        .WithMany("DimensionFacts")
                        .HasForeignKey("FactId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DimensionValue");

                    b.Navigation("Fact");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.Level", b =>
                {
                    b.HasOne("SoccerPlayerApi.Entities.Structure.Level", "Ancestor")
                        .WithMany("Children")
                        .HasForeignKey("AncestorId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("SoccerPlayerApi.Entities.Structure.Dimension", "Dimension")
                        .WithMany("Levels")
                        .HasForeignKey("DimensionId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Ancestor");

                    b.Navigation("Dimension");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.Dimension", b =>
                {
                    b.Navigation("Levels");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.Fact", b =>
                {
                    b.Navigation("DimensionFacts");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.Level", b =>
                {
                    b.Navigation("Children");

                    b.Navigation("DimensionValues");

                    b.Navigation("Environment1s");

                    b.Navigation("Environment2s");

                    b.Navigation("Environment3s");

                    b.Navigation("Environment4s");

                    b.Navigation("Environment5s");
                });
#pragma warning restore 612, 618
        }
    }
}
