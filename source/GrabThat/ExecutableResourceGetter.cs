using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace GrabThat;

internal class ExecutableResourceGetter : ResourceGetter
{
    internal ExecutableResourceGetter(FilePath path) : base(path) { }

    public override async Task<Resource> GetResourceAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(Path))
        {
            throw new FileNotFoundException($"The file '{Path}' does not exist.");
        }

        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = Path,
                UseShellExecute = false,
                CreateNoWindow = true
            },
            EnableRaisingEvents = true
        };
       
        using var _ = cancellationToken.Register(() =>
        {
            if (!process.HasExited)
            {
                process.Kill();
            }
        });

        process.Start();            
        await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);
        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException($"The executable '{Path}' exited with code {process.ExitCode}.");
        }

        var output = await process.StandardOutput.ReadToEndAsync(cancellationToken).ConfigureAwait(false);
        var content = JsonSerializer.Deserialize<JsonNode>(output, DefaultJsonSerializerOptions);
        return new Resource(content);
    } 
}
