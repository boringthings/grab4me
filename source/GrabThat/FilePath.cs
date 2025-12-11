// Copyright (c) 2025
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

internal record FilePath(string Path)
{
    public static implicit operator string(FilePath filePath) => filePath.Path;

    public static implicit operator FilePath(string path) => new(path);

    public override string ToString() => Path;
}