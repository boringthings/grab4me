using System.Threading;
using System.Threading.Tasks;

namespace GrabThat;

internal interface IResourceGetter
{
    FilePath Path { get; }

    Task<Resource> GetResourceAsync(CancellationToken cancellationToken);
}
