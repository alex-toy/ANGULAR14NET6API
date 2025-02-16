using Microsoft.EntityFrameworkCore;
using SoccerPlayerApi.Entities.Structure;
using SoccerPlayerApi.Repo;
using SoccerPlayerApi.Repo.Generics;
using SoccerPlayerApi.Services.Aggregations;
using SoccerPlayerApi.Services.Dimensions;
using SoccerPlayerApi.Services.Facts;
using SoccerPlayerApi.Services.Frames;
using SoccerPlayerApi.Services.Imports;
using SoccerPlayerApi.Services.Levels;
using SoccerPlayerApi.Services.Settings;
using SoccerPlayerApi.Services.Simulations;

namespace SoccerPlayerApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        string connectionString = builder.Configuration.GetConnectionString("default")!;
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

        builder.Services.AddScoped<IGenericRepo<Dimension>, GenericRepo<Dimension>>();
        builder.Services.AddScoped<IGenericRepo<Fact>, GenericRepo<Fact>>();
        builder.Services.AddScoped<IGenericRepo<Aggregation>, GenericRepo<Aggregation>>();
        builder.Services.AddScoped<IDimensionService, DimensionService>();
        builder.Services.AddScoped<IAggregationService, AggregationService>();
        builder.Services.AddScoped<IFactService, FactService>();
        builder.Services.AddScoped<ILevelService, LevelService>();
        builder.Services.AddScoped<IFrameService, FrameService>();
        builder.Services.AddScoped<ISettingsService, SettingsService>();
        builder.Services.AddScoped<IImportService, ImportService>();
        builder.Services.AddScoped<ISimulationService, SimulationService>();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });




        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            CreateType(dbContext, "ExecutionLogs");
            CreateType(dbContext, "ImportFactType");
            CreateStoredProcedure(dbContext, "GetTimeSeries");
            CreateStoredProcedure(dbContext, "CreateImportFacts");
            CreateStoredProcedure(dbContext, "SetFrameSorting");
            CreateStoredProcedure(dbContext, "create_time_aggregations");
            CreateStoredProcedure(dbContext, "generate_dates");

            ExecuteGenerate_date_series(dbContext);
            ExecuteCreate_time_aggregations(dbContext);
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.UseCors("CorsPolicy");

        app.Run();
    }

    private static void CreateType(ApplicationDbContext dbContext, string name)
    {
        var sqlFilePath = Path.Combine(Directory.GetCurrentDirectory(), $"SQL\\Types\\{name}.sql");
        if (File.Exists(sqlFilePath))
        {
            var sql = File.ReadAllText(sqlFilePath);
            dbContext.Database.ExecuteSqlRaw(sql);
        }
        else
        {
            throw new FileNotFoundException($"SQL file '{sqlFilePath}' not found.");
        }
    }

    private static void CreateStoredProcedure(ApplicationDbContext dbContext, string name)
    {
        var sqlFilePath = Path.Combine(Directory.GetCurrentDirectory(), $"SQL\\stored_procedures\\{name}.sql");
        if (File.Exists(sqlFilePath))
        {
            var sql = File.ReadAllText(sqlFilePath);
            dbContext.Database.ExecuteSqlRaw(sql);
        }
        else
        {
            throw new FileNotFoundException($"SQL file '{sqlFilePath}' not found.");
        }
    }

    public static void ExecuteGenerate_date_series(ApplicationDbContext dbContext)
    {
        dbContext.Database.ExecuteSqlRaw("EXEC generate_date_series '2022-01-01', '2026-12-31';");
    }

    public static void ExecuteCreate_time_aggregations(ApplicationDbContext dbContext)
    {
        // erruer quand executé une deuxième fois
        //dbContext.Database.ExecuteSqlRaw("EXEC create_time_aggregations;");
    }
}
