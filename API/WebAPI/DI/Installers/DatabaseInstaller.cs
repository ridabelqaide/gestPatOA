using PATOA.INFRA.Data;
using Microsoft.EntityFrameworkCore;

namespace PATOA.WebAPI.DI.Installers;

public static class DatabaseInstaller
{
    public static async Task<WebApplication> InitializeDatabaseAsync(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                await context.Database.MigrateAsync();
                app.Logger.LogInformation("Database migration completed successfully");
              //  await services.GetRequiredService<IServiceProvider>().SeedDataAsync();
            }
            catch (Exception ex)
            {
                app.Logger.LogError(ex, "An error occurred while migrating the database");
                throw;
            }
        }
        

        return app;
    }
} 