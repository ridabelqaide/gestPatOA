using Serilog;
using Serilog.Events;

namespace PATOA.WebAPI.DI.Installers;

public static class LoggingInstaller
{
    public static WebApplicationBuilder AddLoggingServices(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Debug)
            .Enrich.FromLogContext()
            .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug)
            .WriteTo.File("logs/PATOA-.txt",
                rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: LogEventLevel.Debug,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        builder.Host.UseSerilog();

        return builder;
    }
} 