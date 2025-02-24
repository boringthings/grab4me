namespace Grab4Me;

using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

internal interface IResourceManager
{
    /// <summary>
    /// Gets the content of resource at given path.
    /// </summary>
    /// <param name="path"></param>
    /// <returns>A new instance of <see cref="JsonDocument"/>.</returns>
    /// <exception cref="FileNotFoundException">Thrown when the resource is not found.</exception>
    /// <exception cref="NotSupportedException">Thrown when the resource is not supported.</exception>
    Task<JsonDocument> GetResourceAsync(string path, CancellationToken cancellationToken);
}
