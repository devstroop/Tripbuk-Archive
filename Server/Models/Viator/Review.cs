using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tripbuk.Server.Models.Viator
{
    public partial class Review
    {
        [JsonPropertyName("sources")]
        public IEnumerable<Source> Sources { get; set; }

        [JsonPropertyName("reviewCountTotals")]
        public IEnumerable<ReviewCount> ReviewCountTotals { get; set; }

        [JsonPropertyName("totalReviews")]
        public int TotalReviews { get; set; }

        [JsonPropertyName("combinedAverageRating")]
        public double CombinedAverageRating { get; set; }
    }
}