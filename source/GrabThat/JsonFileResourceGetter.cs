namespace GrabThat;

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System;

internal class JsonFileResourceGetter : ResourceGetter
{
    public JsonFileResourceGetter(FilePath filePath)
        : base(filePath)
    {
    }

    public override async Task<Resource> GetResourceAsync(CancellationToken cancellationToken = default)
    {
        if (!File.Exists(Path))
        {
            throw new FileNotFoundException($"The file '{Path}' does not exist.");
        }

        try
        {
            await using var stream = File.OpenRead(Path);
            var content =
                await JsonSerializer.DeserializeAsync<object>(stream, cancellationToken: cancellationToken);
            return new Resource(content);
        }
        catch (Exception ex)
        {
            throw new InvalidDataException($"The file '{Path}' is not a valid JSON file.", ex);
        }
    }
}
