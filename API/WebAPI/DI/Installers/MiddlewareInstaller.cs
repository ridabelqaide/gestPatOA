using PATOA.WebAPI.Middleware;

namespace PATOA.WebAPI.DI.Installers;

public static class MiddlewareInstaller
{
    public static WebApplication UseMiddlewareServices(this WebApplication app)
    {
        // Configure middleware pipeline
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }
} 