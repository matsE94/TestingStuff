using System.Net;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation;

namespace Mats.Edvardsen.TestingStuff.Web;

public class ExceptionMiddleware(IHostEnvironment env) : IMiddleware
{
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        // catch (EntityNotFoundException e)
        // {
        //     await WriteToStream(e.Message, e.StackTrace, HttpStatusCode.NotFound, context);
        // }
        catch (ValidationException e)
        {
            await WriteToStream(e.Message, e.StackTrace, HttpStatusCode.BadRequest, context);
        }
        catch (Exception e)
        {
            var message = env.IsDevelopment() ? e.Message : "Something went wrong, see logs for more information.";
            await WriteToStream(message, e.StackTrace, HttpStatusCode.InternalServerError, context);
        }
    }

    private async Task WriteToStream
    (
        string message,
        string? trace,
        HttpStatusCode code,
        HttpContext context
    )
    {
        var response = new ExceptionStreamOutput
        (
            (int)code,
            env.EnvironmentName,
            message,
            env.IsDevelopment() ? trace : null
        );
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = (int)code;

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, _serializerOptions));
    }
}

public record ExceptionStreamOutput(int StatusCode, string Environment, string Message, string? StackTrace = null);