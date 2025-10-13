using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Serilog;
using ILogger = Serilog.ILogger;

namespace PATOA.WebAPI.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = new ErrorResponse();

            switch (exception)
            {
                case KeyNotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Message = "The requested resource was not found.";
                    break;
                case UnauthorizedAccessException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.Message = "Unauthorized access.";
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Message = "An internal server error occurred.";
                    break;
            }

            response.Details = exception.Message;
            
            var result = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(result);
        }
    }

    public class ErrorResponse
    {
        public string Message { get; set; }
        public string Details { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
} 