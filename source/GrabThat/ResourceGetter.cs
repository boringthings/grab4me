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
}
