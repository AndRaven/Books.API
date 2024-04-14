

using System.Net;
using Serilog;

public class CustomExceptionHandlingMidleware
{
    private readonly RequestDelegate _next;

    public CustomExceptionHandlingMidleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        if (exception is ApplicationException)
        {
            Log.ForContext("ValidationError", exception.Message)
               .Warning("Validation error occurred in API.");
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return context.Response.WriteAsJsonAsync(new { exception.Message });
        }
        else
        {
            var errorId = Guid.NewGuid();
            Log.ForContext("ErrorId", errorId)
               .Error(exception, "Error occured in API");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsJsonAsync(new
            {
                ErrorId = errorId,
                Message = "Something bad happened in our API."
            });
        }
    }
}