using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tripbuk.Server.Models.Viator
{
    public partial class ProductReviewsSummary
    {
        [JsonPropertyName("sources")]
        public IEnumerable<ProductReviewsSummarySource> Sources { get; set; }

        [JsonPropertyName("reviewCountTotals")]
        public IEnumerable<ProductReviewsSummaryCount> ReviewCountTotals { get; set; }

        [JsonPropertyName("totalReviews")]
        public int TotalReviews { get; set; }

        [JsonPropertyName("combinedAverageRating")]
        public double CombinedAverageRating { get; set; }
    }
    
    public partial class ProductReviewsSummaryCount
    {
        [JsonPropertyName("rating")]
        public double Rating { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }
    }
    
    public partial class ProductReviewsSummarySource
    {
        [JsonPropertyName("provider")]
        public string Provider { get; set; }

        [JsonPropertyName("reviewCounts")]
        public IEnumerable<ProductReviewsSummaryCount> ReviewCounts { get; set; }

        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }

        [JsonPropertyName("averageRating")]
        public double AverageRating { get; set; }
    }
}