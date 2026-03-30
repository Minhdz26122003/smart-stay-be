using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SmartStay.Application.Common.Models;
using System.Text.Json;
using SmartStay.Domain.Exceptions;

namespace SmartStay.API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception has occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = exception switch
        {
            NotFoundException e => (StatusCode: (int)HttpStatusCode.NotFound, Message: e.Message),
            UnauthorizedException e => (StatusCode: (int)HttpStatusCode.Unauthorized, Message: e.Message),
            ConflictException e => (StatusCode: (int)HttpStatusCode.Conflict, Message: e.Message),
            _ => (StatusCode: (int)HttpStatusCode.InternalServerError, Message: "An unexpected error occurred.")
        };

        context.Response.StatusCode = response.StatusCode;

        var apiResponse = ApiResponse<object>.Fail(response.Message);
        
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(apiResponse, options);

        await context.Response.WriteAsync(json);
    }
}
