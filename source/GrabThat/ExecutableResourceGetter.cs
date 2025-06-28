
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace GrabThat;

internal class ExecutableResourceGetter : ResourceGetter
{
    internal ExecutableResourceGetter(FilePath path) : base(path) { }

    public override Task<Resource> GetResourceAsync(CancellationToken _)
    {
        if (!File.Exists(Path))
        {
            throw new FileNotFoundException($"The file '{Path}' does not exist.");
        }

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = Path,
                UseShellExecute = false,
                CreateNoWindow = true
            },
            EnableRaisingEvents = true
        };

        var tcs = new TaskCompletionSource<Resource>();

        process.Exited += (sender, args) =>
        {
            if (process.ExitCode != 0)
            {
                tcs.SetException(
                    new InvalidOperationException(
                        $"{System.IO.Path.GetFileNameWithoutExtension(Path)} exited with code {process.ExitCode}."));
                return;
            }
            try
            {
                var output = process.StandardOutput.ReadToEnd();
                tcs.SetResult(new Resource(output));
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        };

        try
        {
            process.Start();
        }
        catch (Exception ex)
        {
            tcs.SetException(ex);
        }
    
        return tcs.Task;
    }
}
