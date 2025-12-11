// Copyright (c) 2025
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

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
        RootPath = new FilePath(path);
    }

    internal FilePath RootPath { get; init; }

    public IResourceGetter Create(FilePath relativePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(relativePath);

        FilePath resourcePath = Path.Combine(RootPath, relativePath);
        if (!File.Exists(resourcePath))
        {
            throw new FileNotFoundException($"The file '{resourcePath}' does not exist.");
        }

        var fileMode = File.GetUnixFileMode(resourcePath);
        if ((fileMode & UnixFileMode.UserExecute) != 0)
        {
            return new ExecutableResourceGetter(resourcePath);
        }

        return new JsonFileResourceGetter(resourcePath);
    }
}
