namespace Mats.Edvardsen.TestingStuff.Web;

public interface IWebAssemblyMarker;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.RegisterServices(builder.Configuration);

        var app = builder.Build();

        app.UseMiddleware<ExceptionMiddleware>();
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.MapControllers();

        await app.RunAsync();
    }
}