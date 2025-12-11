// Copyright (c) 2025
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace GrabThat;

internal interface IResourceGetterFactory
{
    IResourceGetter Create(FilePath path);
}
