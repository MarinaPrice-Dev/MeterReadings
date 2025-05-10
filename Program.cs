using MeterReadings.Data;
using MeterReadings.Repositories;
using MeterReadings.Repositories.SeedData;
using MeterReadings.Services;
using MeterReadings.Services.Validation;
using Microsoft.EntityFrameworkCore;

namespace MeterReadings;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        
        // Register services
        builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();
        builder.Services.AddScoped<ICsvParserService, CsvParserService>();
        builder.Services.AddScoped<IMeterReadingService, MeterReadingService>();
        builder.Services.AddScoped<IMeterReadingValidator, MeterReadingValidator>();
        // Register repositories
        builder.Services.AddScoped<IAccountRepository, AccountRepository>();
        builder.Services.AddScoped<IMeterReadingRepository, MeterReadingRepository>();
        builder.Services.AddScoped<IDataSeeder, TestAccountSeeder>();
        
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        SetupCorsPolicy(builder);
        
        var app = builder.Build();

        // Configure middleware
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("AllowFrontend"); 
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        
        ApplyMigrations(app);
        await SeedDatabaseAsync(app);
        
        app.Run();
    }

    private static void SetupCorsPolicy(WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins("http://localhost:5173") // Vue dev server
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }

    private static void ApplyMigrations(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.Migrate();
        }
    }

    private static async Task SeedDatabaseAsync(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            using var scope = app.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
            await seeder.SeedAsync();
        }
    }
}