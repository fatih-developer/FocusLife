using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using FocusLifePlus.Application.Common.Exceptions;

namespace FocusLifePlus.Application.Common.Middleware;

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
            _logger.LogError(ex, "An unhandled exception occurred: {Message}, StackTrace: {StackTrace}", ex.Message, ex.StackTrace);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        if (!context.Response.HasStarted)
        {
            context.Response.ContentType = "application/json";

            var response = new Dictionary<string, object>();

            var statusCode = exception switch
            {
                FocusLifePlus.Application.Common.Exceptions.ValidationException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            context.Response.StatusCode = statusCode;

            response["error"] = exception switch
            {
                FocusLifePlus.Application.Common.Exceptions.ValidationException validationException => new
                {
                    message = "Girdiğiniz bilgilerde hatalar var",
                    validationErrors = validationException.Errors
                },
                NotFoundException notFoundException => new
                {
                    message = notFoundException.Message
                },
                _ => new
                {
                    message = "İşleminiz gerçekleştirilirken bir hata oluştu.",
                    details = exception.Message,
                    stackTrace = exception.StackTrace
                }
            };

            var result = JsonSerializer.Serialize(response, new JsonSerializerOptions 
            { 
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(result);
        }
    }
} 