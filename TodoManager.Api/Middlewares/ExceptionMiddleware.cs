using LoggerService;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using TodoManager.Shared.CustomExceptions;
using TodoManager.Shared;
using static System.Net.Mime.MediaTypeNames;

namespace TodoManager.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;

        public ExceptionMiddleware(RequestDelegate next, ILoggerManager logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.ContentType = Application.Json;

            httpContext.Response.StatusCode = ex switch
            {
                ApiException => ((ApiException)ex).StatusCode,
                _ => StatusCodes.Status500InternalServerError
            };

            await httpContext.Response.WriteAsync(new ErrorResponse
            {
                StatusCode = httpContext.Response.StatusCode,
                ErrorMessage = ex.Message
            }.ToString());
            
        }
    }
}
