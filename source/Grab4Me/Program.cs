namespace Grab4Me;

using Microsoft.AspNetCore.Builder;

public class Program
{
    public static int Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.MapGet("/", () => "Hello World!"); //TODO: list all available resources
        app.MapGet("/{*path}", (string path) => $"Hello {path}!"); // TODO get the resource


        app.Run("http://*:59000");

        return 0;
    }
}
