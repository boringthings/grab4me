namespace GrabThat;

using System;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;

internal class ExecutableResourceGetter : ResourceGetter
{
    internal ExecutableResourceGetter(FilePath path) : base(path) { }

    public override Task<Resource> GetResourceAsync(CancellationToken cancellationToken = default)
    {
        // Implementation for finding an executable resource
        throw new NotImplementedException();
    }
}
