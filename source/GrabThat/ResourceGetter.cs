using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace GrabThat;

internal abstract class ResourceGetter : IResourceGetter
{
    internal ResourceGetter(FilePath path)
    {
        Path = path;
    }

    public FilePath Path { get; init; }

    public abstract Task<Resource> GetResourceAsync(CancellationToken cancellationToken);

    protected readonly JsonSerializerOptions DefaultJsonSerializerOptions = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        AllowTrailingCommas = true,
        Converters = 
        {
            new JsonStringEnumConverter()
        }
    };
}
