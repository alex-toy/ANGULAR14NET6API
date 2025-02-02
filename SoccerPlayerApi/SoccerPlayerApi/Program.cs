using Microsoft.EntityFrameworkCore;
using SoccerPlayerApi.Entities.Structure;
using SoccerPlayerApi.Repo;
using SoccerPlayerApi.Repo.Generics;
using SoccerPlayerApi.Services.Aggregations;
using SoccerPlayerApi.Services.Dimensions;
using SoccerPlayerApi.Services.Environments;
using SoccerPlayerApi.Services.Facts;
using SoccerPlayerApi.Services.Levels;
using SoccerPlayerApi.Services.Settings;

namespace SoccerPlayerApi
{
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

            builder.Services.AddScoped<IGenericRepo<Dimension>, GenericRepo<Dimension>>();
            builder.Services.AddScoped<IGenericRepo<Fact>, GenericRepo<Fact>>();
            builder.Services.AddScoped<IGenericRepo<Aggregation>, GenericRepo<Aggregation>>();
            builder.Services.AddScoped<IDimensionService, DimensionService>();
            builder.Services.AddScoped<IAggregationService, AggregationService>();
            builder.Services.AddScoped<IFactService, FactService>();
            builder.Services.AddScoped<ILevelService, LevelService>();
            builder.Services.AddScoped<IEnvironmentService, EnvironmentService>();
            builder.Services.AddScoped<ISettingsService, SettingsService>();

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

            //using (var scope = app.Services.CreateScope())
            //{
            //    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            //    dbContext.Database.Migrate();
            //}

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
    }
}
