using System.Text.Json;

namespace DeskFlow.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var erro = new
            {
                mensagem = "Ocorreu um erro interno no servidor.",
                detalhe = ex.Message
            };

            var json = JsonSerializer.Serialize(erro);
            await context.Response.WriteAsync(json);
        }
    }
}