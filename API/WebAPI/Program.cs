using PATOA.INFRA.Data;
using PATOA.INFRA.Settings;
using PATOA.WebAPI.DI;
using PATOA.WebAPI.DI.Installers;
using PATOA.WebAPI.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Text;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug)
    .WriteTo.File("logs/patoa-.txt", 
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: LogEventLevel.Debug,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    // Add logging
    builder.AddLoggingServices();
    // Add JWT settings
    builder.Services.Configure<JwtSettings>(
        builder.Configuration.GetSection("Jwt"));

    // Add services to the container
    builder.Services.AddApplicationServices(builder.Configuration);
    builder.Services.AddAuthenticationServices(builder.Configuration);
    builder.Services.AddSwaggerServices();
    builder.Services.AddControllers();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    // Configure CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("front", policy =>
        {
            policy.WithOrigins("https://localhost:4200",
                "http://localhost:4200", 
                "http://localhost:81",
                "http://192.168.1.12:81/",
                "http://server:81")
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
    });
    
    var app = builder.Build();

    // Initialize database
    await app.InitializeDatabaseAsync();

    // Configure Swagger
    app.UseSwaggerServices();
    
    // Configure CORS - IMPORTANT: UseCors must be called before UseAuthentication and UseAuthorization
    app.UseCors("front");
    
    // Configure middleware pipeline
    app.UseMiddlewareServices();

    Log.Information("Starting up API...");
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
    throw;
}
finally
{
    Log.Information("Shutting down application");
    Log.CloseAndFlush();
}
