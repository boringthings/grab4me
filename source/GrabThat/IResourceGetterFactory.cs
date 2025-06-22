namespace GrabThat;

internal interface IResourceGetterFactory
{
    IResourceGetter Create(FilePath path);
}
