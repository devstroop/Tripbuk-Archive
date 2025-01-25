using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tripbuk.Server.Models.Viator
{
    public partial class ReviewCount
    {
        [JsonPropertyName("rating")]
        public double Rating { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }
    }
}