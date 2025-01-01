﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SoccerPlayerApi.Repo;

#nullable disable

namespace SoccerPlayerApi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250101132354_facts")]
    partial class facts
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.36")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

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

                    b.ToTable("DimensionFact");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.DimensionValue", b =>
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

                    b.ToTable("DimensionValues");
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

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.DimensionFact", b =>
                {
                    b.HasOne("SoccerPlayerApi.Entities.Structure.DimensionValue", "DimensionValue")
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

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.DimensionValue", b =>
                {
                    b.HasOne("SoccerPlayerApi.Entities.Structure.Level", "Level")
                        .WithMany()
                        .HasForeignKey("LevelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Level");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.Level", b =>
                {
                    b.HasOne("SoccerPlayerApi.Entities.Structure.Level", "Ancestor")
                        .WithMany("Children")
                        .HasForeignKey("AncestorId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("SoccerPlayerApi.Entities.Structure.Dimension", "Dimension")
                        .WithMany()
                        .HasForeignKey("DimensionId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Ancestor");

                    b.Navigation("Dimension");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.Fact", b =>
                {
                    b.Navigation("DimensionFacts");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.Level", b =>
                {
                    b.Navigation("Children");
                });
#pragma warning restore 612, 618
        }
    }
}
