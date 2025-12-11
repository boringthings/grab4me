// Copyright (c) 2025
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;

namespace GrabThat;

internal interface IResourceGetter
{
    FilePath Path { get; }

    Task<Resource> GetResourceAsync(CancellationToken cancellationToken);
}
