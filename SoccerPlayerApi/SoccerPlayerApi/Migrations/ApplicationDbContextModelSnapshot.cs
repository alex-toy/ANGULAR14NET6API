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

            modelBuilder.Entity("SoccerPlayerApi.Entities.Frames.Frame", b =>
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

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LevelIdFilter1");

                    b.HasIndex("LevelIdFilter2");

                    b.HasIndex("LevelIdFilter3");

                    b.HasIndex("LevelIdFilter4");

                    b.ToTable("Environments");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Frames.FrameScope", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Dimension1AggregationId")
                        .HasColumnType("int");

                    b.Property<int>("Dimension1Id")
                        .HasColumnType("int");

                    b.Property<int?>("Dimension2AggregationId")
                        .HasColumnType("int");

                    b.Property<int?>("Dimension2Id")
                        .HasColumnType("int");

                    b.Property<int?>("Dimension3AggregationId")
                        .HasColumnType("int");

                    b.Property<int?>("Dimension3Id")
                        .HasColumnType("int");

                    b.Property<int?>("Dimension4AggregationId")
                        .HasColumnType("int");

                    b.Property<int?>("Dimension4Id")
                        .HasColumnType("int");

                    b.Property<int>("FrameId")
                        .HasColumnType("int");

                    b.Property<string>("SortingValue")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Dimension1AggregationId");

                    b.HasIndex("Dimension2AggregationId");

                    b.HasIndex("Dimension3AggregationId");

                    b.HasIndex("Dimension4AggregationId");

                    b.HasIndex("FrameId");

                    b.ToTable("EnvironmentScopes");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Frames.FrameSorting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Aggregator")
                        .HasColumnType("int");

                    b.Property<int>("DataTypeId")
                        .HasColumnType("int");

                    b.Property<int>("EndTimeSpan")
                        .HasColumnType("int");

                    b.Property<int>("FrameId")
                        .HasColumnType("int");

                    b.Property<int>("IsAscending")
                        .HasColumnType("int");

                    b.Property<int>("OrderIndex")
                        .HasColumnType("int");

                    b.Property<int>("StartTimeSpan")
                        .HasColumnType("int");

                    b.Property<int>("TimeLevelId")
                        .HasColumnType("int");

                    b.Property<int>("TimeSpanBase")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DataTypeId");

                    b.HasIndex("FrameId");

                    b.ToTable("EnvironmentSortings");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Setting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Key")
                        .IsUnique();

                    b.ToTable("Settings");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Key = "PresentWeekDate",
                            Value = "2024-W09"
                        },
                        new
                        {
                            Id = 2,
                            Key = "PastWeekSpan",
                            Value = "10"
                        },
                        new
                        {
                            Id = 3,
                            Key = "PresentMonthDate",
                            Value = "2024-M09"
                        },
                        new
                        {
                            Id = 4,
                            Key = "PastMonthSpan",
                            Value = "10"
                        });
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Simulations.Algorithms.Algorithm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Algorithms");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Label = "Average"
                        });
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Simulations.Algorithms.AlgorithmParameterKey", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AlgorithmId")
                        .HasColumnType("int");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AlgorithmId");

                    b.ToTable("AlgorithmParameterKeys");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AlgorithmId = 1,
                            Key = "alpha"
                        });
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Simulations.Algorithms.AlgorithmParameterValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AlgorithmId")
                        .HasColumnType("int");

                    b.Property<int>("AlgorithmParameterKeyId")
                        .HasColumnType("int");

                    b.Property<int?>("FrameSimulationId")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FrameSimulationId");

                    b.ToTable("AlgorithmParameterValues");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Simulations.FrameSimulation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AlgorithmId")
                        .HasColumnType("int");

                    b.Property<int>("FrameId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AlgorithmId");

                    b.HasIndex("FrameId");

                    b.ToTable("Simulations");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Simulations.SimulationFact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<decimal>("Amount")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("FrameSimulationId")
                        .HasColumnType("int");

                    b.Property<int>("SimulationId")
                        .HasColumnType("int");

                    b.Property<int>("TimeAggregationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FrameSimulationId");

                    b.ToTable("SimulationFacts");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.Aggregation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("LevelId")
                        .HasColumnType("int");

                    b.Property<int?>("MotherAggregationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Label")
                        .IsUnique();

                    b.HasIndex("LevelId");

                    b.HasIndex("MotherAggregationId");

                    b.ToTable("Aggregations");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Label = "all-client",
                            LevelId = 1
                        },
                        new
                        {
                            Id = 2,
                            Label = "carrefour",
                            LevelId = 2,
                            MotherAggregationId = 1
                        },
                        new
                        {
                            Id = 3,
                            Label = "auchan",
                            LevelId = 2,
                            MotherAggregationId = 1
                        },
                        new
                        {
                            Id = 4,
                            Label = "all-location",
                            LevelId = 7
                        },
                        new
                        {
                            Id = 5,
                            Label = "france",
                            LevelId = 8,
                            MotherAggregationId = 4
                        },
                        new
                        {
                            Id = 6,
                            Label = "espagne",
                            LevelId = 8,
                            MotherAggregationId = 4
                        },
                        new
                        {
                            Id = 7,
                            Label = "all-product",
                            LevelId = 4
                        },
                        new
                        {
                            Id = 8,
                            Label = "home",
                            LevelId = 5,
                            MotherAggregationId = 7
                        },
                        new
                        {
                            Id = 9,
                            Label = "sport",
                            LevelId = 5,
                            MotherAggregationId = 7
                        });
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.DataType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DataTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Label = "sales"
                        });
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.Dimension", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Label")
                        .IsUnique();

                    b.ToTable("Dimensions");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Label = "client"
                        },
                        new
                        {
                            Id = 2,
                            Label = "product"
                        },
                        new
                        {
                            Id = 3,
                            Label = "location"
                        });
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.Fact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("Aggregation4Id")
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("DataTypeId")
                        .HasColumnType("int");

                    b.Property<int>("Dimension1AggregationId")
                        .HasColumnType("int");

                    b.Property<int?>("Dimension2AggregationId")
                        .HasColumnType("int");

                    b.Property<int?>("Dimension3AggregationId")
                        .HasColumnType("int");

                    b.Property<int?>("Dimension4AggregationId")
                        .HasColumnType("int");

                    b.Property<int>("TimeAggregationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Aggregation4Id");

                    b.HasIndex("DataTypeId");

                    b.HasIndex("Dimension2AggregationId");

                    b.HasIndex("Dimension3AggregationId");

                    b.HasIndex("TimeAggregationId");

                    b.HasIndex("Dimension1AggregationId", "Dimension2AggregationId", "Dimension3AggregationId", "Dimension4AggregationId", "TimeAggregationId", "DataTypeId")
                        .IsUnique()
                        .HasFilter("[Dimension2AggregationId] IS NOT NULL AND [Dimension3AggregationId] IS NOT NULL AND [Dimension4AggregationId] IS NOT NULL");

                    b.ToTable("Facts");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.Level", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("DimensionId")
                        .HasColumnType("int");

                    b.Property<int?>("FatherId")
                        .HasColumnType("int");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DimensionId");

                    b.HasIndex("FatherId");

                    b.ToTable("Levels");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DimensionId = 1,
                            Label = "all-client"
                        },
                        new
                        {
                            Id = 2,
                            DimensionId = 1,
                            FatherId = 1,
                            Label = "main client"
                        },
                        new
                        {
                            Id = 3,
                            DimensionId = 1,
                            FatherId = 2,
                            Label = "client sku"
                        },
                        new
                        {
                            Id = 4,
                            DimensionId = 2,
                            Label = "all-product"
                        },
                        new
                        {
                            Id = 5,
                            DimensionId = 2,
                            FatherId = 4,
                            Label = "family"
                        },
                        new
                        {
                            Id = 6,
                            DimensionId = 2,
                            FatherId = 6,
                            Label = "product sku"
                        },
                        new
                        {
                            Id = 7,
                            DimensionId = 3,
                            Label = "all-location"
                        },
                        new
                        {
                            Id = 8,
                            DimensionId = 3,
                            FatherId = 7,
                            Label = "country"
                        },
                        new
                        {
                            Id = 9,
                            DimensionId = 3,
                            FatherId = 8,
                            Label = "city"
                        });
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.TimeAggregation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MotherAggregationId")
                        .HasColumnType("int");

                    b.Property<int>("TimeLevelId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MotherAggregationId");

                    b.HasIndex("TimeLevelId");

                    b.ToTable("TimeAggregations");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.TimeDimension", b =>
                {
                    b.Property<DateTime>("Day")
                        .HasColumnType("datetime2")
                        .HasColumnName("_day");

                    b.Property<int>("Month")
                        .HasColumnType("int")
                        .HasColumnName("_month");

                    b.Property<string>("MonthLabel")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasColumnName("_month_label");

                    b.Property<int>("Semester")
                        .HasColumnType("int")
                        .HasColumnName("_semester");

                    b.Property<string>("SemesterLabel")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasColumnName("_semester_label");

                    b.Property<int>("Trimester")
                        .HasColumnType("int")
                        .HasColumnName("_trimester");

                    b.Property<string>("TrimesterLabel")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasColumnName("_trimester_label");

                    b.Property<int>("Week")
                        .HasColumnType("int")
                        .HasColumnName("_week");

                    b.Property<string>("WeekLabel")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasColumnName("_week_label");

                    b.Property<int>("Year")
                        .HasColumnType("int")
                        .HasColumnName("_year");

                    b.HasKey("Day");

                    b.ToTable("DateSeries", (string)null);
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.TimeLevel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("AncestorId")
                        .HasColumnType("int");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AncestorId");

                    b.ToTable("TimeLevels");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Label = "YEAR"
                        },
                        new
                        {
                            Id = 2,
                            AncestorId = 1,
                            Label = "SEMESTER"
                        },
                        new
                        {
                            Id = 3,
                            AncestorId = 2,
                            Label = "TRIMESTER"
                        },
                        new
                        {
                            Id = 4,
                            AncestorId = 3,
                            Label = "MONTH"
                        },
                        new
                        {
                            Id = 5,
                            AncestorId = 4,
                            Label = "WEEK"
                        });
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Frames.Frame", b =>
                {
                    b.HasOne("SoccerPlayerApi.Entities.Structure.Level", "LevelFilter1")
                        .WithMany("Frame1s")
                        .HasForeignKey("LevelIdFilter1")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SoccerPlayerApi.Entities.Structure.Level", "LevelFilter2")
                        .WithMany("Frame2s")
                        .HasForeignKey("LevelIdFilter2")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("SoccerPlayerApi.Entities.Structure.Level", "LevelFilter3")
                        .WithMany("Frame3s")
                        .HasForeignKey("LevelIdFilter3")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("SoccerPlayerApi.Entities.Structure.Level", "LevelFilter4")
                        .WithMany("Frame4s")
                        .HasForeignKey("LevelIdFilter4")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("LevelFilter1");

                    b.Navigation("LevelFilter2");

                    b.Navigation("LevelFilter3");

                    b.Navigation("LevelFilter4");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Frames.FrameScope", b =>
                {
                    b.HasOne("SoccerPlayerApi.Entities.Structure.Aggregation", "Dimension1Aggregation")
                        .WithMany()
                        .HasForeignKey("Dimension1AggregationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SoccerPlayerApi.Entities.Structure.Aggregation", "Dimension2Aggregation")
                        .WithMany()
                        .HasForeignKey("Dimension2AggregationId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("SoccerPlayerApi.Entities.Structure.Aggregation", "Dimension3Aggregation")
                        .WithMany()
                        .HasForeignKey("Dimension3AggregationId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("SoccerPlayerApi.Entities.Structure.Aggregation", "Dimension4Aggregation")
                        .WithMany()
                        .HasForeignKey("Dimension4AggregationId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("SoccerPlayerApi.Entities.Frames.Frame", "Frame")
                        .WithMany("FrameScopes")
                        .HasForeignKey("FrameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dimension1Aggregation");

                    b.Navigation("Dimension2Aggregation");

                    b.Navigation("Dimension3Aggregation");

                    b.Navigation("Dimension4Aggregation");

                    b.Navigation("Frame");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Frames.FrameSorting", b =>
                {
                    b.HasOne("SoccerPlayerApi.Entities.Structure.DataType", "DataType")
                        .WithMany()
                        .HasForeignKey("DataTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SoccerPlayerApi.Entities.Frames.Frame", "Frame")
                        .WithMany("FrameSortings")
                        .HasForeignKey("FrameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DataType");

                    b.Navigation("Frame");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Simulations.Algorithms.AlgorithmParameterKey", b =>
                {
                    b.HasOne("SoccerPlayerApi.Entities.Simulations.Algorithms.Algorithm", null)
                        .WithMany("Keys")
                        .HasForeignKey("AlgorithmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Simulations.Algorithms.AlgorithmParameterValue", b =>
                {
                    b.HasOne("SoccerPlayerApi.Entities.Simulations.FrameSimulation", null)
                        .WithMany("Values")
                        .HasForeignKey("FrameSimulationId");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Simulations.FrameSimulation", b =>
                {
                    b.HasOne("SoccerPlayerApi.Entities.Simulations.Algorithms.Algorithm", "Algorithm")
                        .WithMany()
                        .HasForeignKey("AlgorithmId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SoccerPlayerApi.Entities.Frames.Frame", "Frame")
                        .WithMany()
                        .HasForeignKey("FrameId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Algorithm");

                    b.Navigation("Frame");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Simulations.SimulationFact", b =>
                {
                    b.HasOne("SoccerPlayerApi.Entities.Simulations.FrameSimulation", null)
                        .WithMany("SimulationFacts")
                        .HasForeignKey("FrameSimulationId");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.Aggregation", b =>
                {
                    b.HasOne("SoccerPlayerApi.Entities.Structure.Level", "Level")
                        .WithMany("DimensionValues")
                        .HasForeignKey("LevelId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SoccerPlayerApi.Entities.Structure.Aggregation", "MotherAggregation")
                        .WithMany()
                        .HasForeignKey("MotherAggregationId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Level");

                    b.Navigation("MotherAggregation");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.Fact", b =>
                {
                    b.HasOne("SoccerPlayerApi.Entities.Structure.Aggregation", "Aggregation4")
                        .WithMany()
                        .HasForeignKey("Aggregation4Id");

                    b.HasOne("SoccerPlayerApi.Entities.Structure.DataType", "DataType")
                        .WithMany("Facts")
                        .HasForeignKey("DataTypeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SoccerPlayerApi.Entities.Structure.Aggregation", "Aggregation1")
                        .WithMany()
                        .HasForeignKey("Dimension1AggregationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SoccerPlayerApi.Entities.Structure.Aggregation", "Aggregation2")
                        .WithMany()
                        .HasForeignKey("Dimension2AggregationId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("SoccerPlayerApi.Entities.Structure.Aggregation", "Aggregation3")
                        .WithMany()
                        .HasForeignKey("Dimension3AggregationId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("SoccerPlayerApi.Entities.Structure.TimeAggregation", "TimeAggregation")
                        .WithMany()
                        .HasForeignKey("TimeAggregationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Aggregation1");

                    b.Navigation("Aggregation2");

                    b.Navigation("Aggregation3");

                    b.Navigation("Aggregation4");

                    b.Navigation("DataType");

                    b.Navigation("TimeAggregation");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.Level", b =>
                {
                    b.HasOne("SoccerPlayerApi.Entities.Structure.Dimension", "Dimension")
                        .WithMany("Levels")
                        .HasForeignKey("DimensionId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SoccerPlayerApi.Entities.Structure.Level", "Father")
                        .WithMany("Children")
                        .HasForeignKey("FatherId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Dimension");

                    b.Navigation("Father");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.TimeAggregation", b =>
                {
                    b.HasOne("SoccerPlayerApi.Entities.Structure.TimeAggregation", "MotherAggregation")
                        .WithMany()
                        .HasForeignKey("MotherAggregationId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("SoccerPlayerApi.Entities.Structure.TimeLevel", "TimeLevel")
                        .WithMany()
                        .HasForeignKey("TimeLevelId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("MotherAggregation");

                    b.Navigation("TimeLevel");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.TimeLevel", b =>
                {
                    b.HasOne("SoccerPlayerApi.Entities.Structure.TimeLevel", "Ancestor")
                        .WithMany()
                        .HasForeignKey("AncestorId");

                    b.Navigation("Ancestor");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Frames.Frame", b =>
                {
                    b.Navigation("FrameScopes");

                    b.Navigation("FrameSortings");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Simulations.Algorithms.Algorithm", b =>
                {
                    b.Navigation("Keys");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Simulations.FrameSimulation", b =>
                {
                    b.Navigation("SimulationFacts");

                    b.Navigation("Values");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.DataType", b =>
                {
                    b.Navigation("Facts");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.Dimension", b =>
                {
                    b.Navigation("Levels");
                });

            modelBuilder.Entity("SoccerPlayerApi.Entities.Structure.Level", b =>
                {
                    b.Navigation("Children");

                    b.Navigation("DimensionValues");

                    b.Navigation("Frame1s");

                    b.Navigation("Frame2s");

                    b.Navigation("Frame3s");

                    b.Navigation("Frame4s");
                });
#pragma warning restore 612, 618
        }
    }
}
