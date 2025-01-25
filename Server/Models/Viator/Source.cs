using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tripbuk.Server.Models.Viator
{
    public partial class Source
    {
        [JsonPropertyName("provider")]
        public string Provider { get; set; }

        [JsonPropertyName("reviewCounts")]
        public IEnumerable<ReviewCount> ReviewCounts { get; set; }

        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }

        [JsonPropertyName("averageRating")]
        public double AverageRating { get; set; }
    }
}