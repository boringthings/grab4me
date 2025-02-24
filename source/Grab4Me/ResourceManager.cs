namespace Grab4Me;

using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.VisualBasic;

internal class ResourceManager : IResourceManager
{
    private IFileLocator _fileLocator;

    public ResourceManager(IFileLocator fileLocator)
    {
        _fileLocator = fileLocator;
    }

    /// <summary>
    /// Gets the content of resource at given path.
    /// </summary>
    /// <param name="path"></param>
    /// <returns>A new instance of <see cref="JsonDocument"/>.</returns>
    /// <exception cref="FileNotFoundException">Thrown when the resource is not found.</exception>
    /// <exception cref="NotSupportedException">Thrown when the resource is not supported.</exception>
    public async Task<JsonDocument?> GetResource(string path, CancellationToken cancellationToken)
    {
        FileSystemInfo fileInfo = _fileLocator.Locate(path);

        if (fileInfo.UnixFileMode == (UnixFileMode.GroupExecute | UnixFileMode.UserExecute | UnixFileMode.OtherExecute))
        {
            // TODO: Execute the file and return the output
            return await _fileRunner.RunAsync(fileInfo.FullName, cancellationToken); 
            throw new NotImplementedException();
        }

        if (true) // TODO: Check if file is text or binary
        {
            return await JsonDocument.ParseAsync(File.ReadAllText(fileInfo.FullName), cancellationToken);
        }

        throw new NotSupportedException("Directories are not supported.");
    }
}

internal interface IResourceManager
{
    /// <summary>
    /// Gets the content of resource at given path.
    /// </summary>
    /// <param name="path"></param>
    /// <returns>A new instance of <see cref="JsonDocument"/>.</returns>
    /// <exception cref="FileNotFoundException">Thrown when the resource is not found.</exception>
    /// <exception cref="NotSupportedException">Thrown when the resource is not supported.</exception>
    JsonDocument GetResource(string path);
}