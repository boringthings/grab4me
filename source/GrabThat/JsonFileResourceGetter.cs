// Copyright (c) 2025
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

namespace GrabThat;

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System;
using System.Text.Json.Nodes;

internal class JsonFileResourceGetter : ResourceGetter
{
    public JsonFileResourceGetter(FilePath filePath)
        : base(filePath)
    {
    }

    public override async Task<Resource> GetResourceAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(Path))
        {
            throw new FileNotFoundException($"The file '{Path}' does not exist.");
        }

        try
        {
            await using var stream = File.OpenRead(Path);
            var content =
                await JsonSerializer.DeserializeAsync<JsonNode>(stream, DefaultJsonSerializerOptions, cancellationToken);
            return new Resource(content);
        }
        catch (Exception ex)
        {
            throw new InvalidDataException($"The file '{Path}' is not a valid JSON file.", ex);
        }
    }
}
