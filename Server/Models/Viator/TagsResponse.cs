using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Tripbuk.Server.Viator;

namespace Tripbuk.Server.Models.Viator
{
    public partial class TagsResponse
    {
        [JsonPropertyName("tags")] public TagWithAllLocalizations Tags { get; set; }
    }
}