namespace GrabThat;

using System;
using System.IO;
using System.Runtime.Versioning;

[SupportedOSPlatform("linux")]
internal class ResourceGetterFactory : IResourceGetterFactory
{
    internal ResourceGetterFactory()
    {
        var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        path = Path.Combine(path, Program.AppName);
        if (!Directory.Exists(path))
        {
            throw new DirectoryNotFoundException($"The directory '{path}' does not exist.");
        }
        _rootPath = new FilePath(path);
    }

    public IResourceGetter Create(FilePath relativePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(relativePath);

        FilePath resourcePath = Path.Combine(_rootPath, relativePath);
        if (!File.Exists(resourcePath))
        {
            throw new FileNotFoundException($"The file '{resourcePath}' does not exist.");
        }

        if (File.GetUnixFileMode(resourcePath)
            == (UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute))
        {
            return new ExecutableResourceGetter(resourcePath);
        }

        return new JsonFileResourceGetter(resourcePath);
    }

    private readonly FilePath _rootPath;
}
