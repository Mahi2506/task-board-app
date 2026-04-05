using System.Text.Json;
using Microsoft.EntityFrameworkCore;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    private static Task HandleException(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        int statusCode = StatusCodes.Status500InternalServerError;
        string message = "Something went wrong";

        switch (ex)
        {
            case ArgumentException:
                statusCode = StatusCodes.Status400BadRequest;
                message = ex.Message;
                break;

            case UnauthorizedAccessException:
                statusCode = StatusCodes.Status401Unauthorized;
                message = ex.Message;
                break;

            case DbUpdateException dbEx:
                statusCode = StatusCodes.Status400BadRequest;

                if (dbEx.InnerException?.Message.Contains("UNIQUE") == true)
                    message = "Email already exists";
                else
                    message = "Database error occurred";

                break;
        }

        context.Response.StatusCode = statusCode;

        var result = JsonSerializer.Serialize(new
        {
            message = message
        });

        return context.Response.WriteAsync(result);
    }
}