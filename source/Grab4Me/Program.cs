namespace Grab4Me;

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

public class Program
{
    public static int Main(string[] args)
    {
        var resourceFinderFactory = new ResourceFinderFactory();
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();
        app.UseExceptionHandler(exceptionHandlerApp
            => exceptionHandlerApp.Run(async context
                => await Results.Problem()
                             .ExecuteAsync(context)));

        app.MapGet("/", () => "Hello World!"); //TODO: list all available resources
        app.MapGet("/{*path}", async (string path) =>
        {
            try
            {
                var resourceFinder = resourceFinderFactory.Create(path);
                var resource = await resourceFinder.FindResourceAsync();
                return Results.Ok(resource);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message, statusCode: 404);
            }
        });


        app.Run("http://*:59000");

        return 0;
    }
}
