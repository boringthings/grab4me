namespace Grab4Me;

using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

internal class ResourceManager : IResourceManager
{
    private IFileLocator _fileLocator;
    private IFileRunner _fileRunner;

    public ResourceManager(IFileLocator fileLocator, IFileRunner fileRunner)
    {
        _fileLocator = fileLocator;
        _fileRunner = fileRunner;
    }

    /// <summary>
    /// Gets the content of resource at given path.
    /// </summary>
    /// <param name="path"></param>
    /// <returns>A new instance of <see cref="JsonDocument"/>.</returns>
    /// <exception cref="FileNotFoundException">Thrown when the resource is not found.</exception>
    /// <exception cref="NotSupportedException">Thrown when the resource is not supported.</exception>
    public async Task<JsonDocument> GetResourceAsync(string path, CancellationToken cancellationToken)
    {
        FileSystemInfo fileInfo = _fileLocator.Locate(path);

        if (fileInfo.UnixFileMode == (UnixFileMode.GroupExecute | UnixFileMode.UserExecute | UnixFileMode.OtherExecute))
        {
            // TODO: Execute the file and return the output
            return await _fileRunner.RunAsync(fileInfo.FullName, cancellationToken); 
        }

        if (true) // TODO: Check if file is text or binary
        {
            return JsonDocument.Parse(File.ReadAllText(fileInfo.FullName));
        }

        throw new NotSupportedException("Directories are not supported.");
    }
}

internal interface IFileRunner
{
    Task<JsonDocument> RunAsync(string path, CancellationToken cancellationToken);
}

internal interface IFileLocator
{
    FileSystemInfo Locate(string path);
}