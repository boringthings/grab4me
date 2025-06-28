namespace GrabThat;

using System;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

[SupportedOSPlatform("linux")]
public class Program
{
    public const string AppName = "grabthat";

    public static int Main(string[] args)
    {
        var resourceGetterFactory = new ResourceGetterFactory();
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();
        app.UseExceptionHandler(exceptionHandlerApp
            => exceptionHandlerApp.Run(async context
                => await Results.Problem()
                             .ExecuteAsync(context)));

        app.MapGet("/", (HttpRequest request, CancellationToken cancellationToken) =>
        {
            // List all available resources.
            var rootPath = resourceGetterFactory.RootPath;
            if (!Directory.Exists(rootPath))
            {
                return Results.Problem("Root path not found.", statusCode: 500);
            }
            return Results.Ok(
                Directory.GetFiles(rootPath, "*", SearchOption.AllDirectories)
                    .Select(filePath =>
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        var path = Path.GetRelativePath(rootPath, filePath);
                        path = Path.Combine(request.PathBase, path);
                        return request.Host + path;
                    }));
        }); 
        app.MapGet("/{*path}", async (string path, CancellationToken cancellationToken) =>
        {
            try
            {
                var resourceGetter = resourceGetterFactory.Create(path);
                var resource = await resourceGetter.GetResourceAsync(cancellationToken);
                return Results.Ok(resource.Value);
            }
            catch (Exception ex)
            {
                var status = ex switch
                {
                    FileNotFoundException => 404,
                    InvalidDataException => 400,
                    _ => 500
                };
                return Results.Problem(ex.Message, statusCode: status);
            }
        });

        app.Run("http://*:59000");

        return 0;
    }
}
