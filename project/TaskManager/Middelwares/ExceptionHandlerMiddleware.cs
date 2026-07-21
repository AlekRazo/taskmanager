using System.Net;
using TaskManager.Common;
using TaskManager.Common.Exceptions;

namespace TaskManager.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (ValidationException ex)
        {
            await HandleException(httpContext, StatusCodes.Status400BadRequest, "Error de validación.", ex.Message);
        }
        catch (UnauthorizedException ex)
        {
            await HandleException(httpContext, StatusCodes.Status401Unauthorized, "No autorizado", ex.Message);
        }
        catch (BusinessException ex)
        {
            await HandleException(httpContext, StatusCodes.Status400BadRequest, "Error en el proceso.", ex.Errors is null ? new List<string>() : ex.Errors.ToList());
        }
        catch (NotFoundException ex)
        {
            await HandleException (httpContext, StatusCodes.Status404NotFound, "Recurso no encontrado.", ex.Message);
        }
        catch (Exception ex)
        {
            await HandleException(httpContext, StatusCodes.Status500InternalServerError, "Ocurrió un error inesperado. Intente más tarde.", ex.Message);
        }
    }

    private static async Task HandleException(HttpContext context, int statusCode, string message, List<string> details)
    {
        var response = ApiResponse<string>.ErrorResponse(details, message, (int) statusCode);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(response);
    }

    private static async Task HandleException(HttpContext context, int statusCode, string message, string detail)
    {
        var response = ApiResponse<string>.ErrorResponse(new List<string> {detail}, message, (int) statusCode);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(response);
    }
}