using System.Threading.Tasks;

namespace Grab4Me;

internal interface IResourceFinderFactory
{
    IResourceFinder Create(string path);
}

internal interface IResourceFinder
{
    Task<Resource> FindResourceAsync();
}
