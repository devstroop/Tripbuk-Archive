using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tripbuk.Server.Models.Viator
{
    public partial class SearchFreeTextResponse
    {
        [JsonPropertyName("destinations")] public SearchFreeTextResponseResult<DestinationSearchResult> Destinations { get; set; }

        [JsonPropertyName("attractions")] public SearchFreeTextResponseResult<AttractionResult> Attractions { get; set; }

        [JsonPropertyName("products")] public SearchFreeTextResponseResult<ProductSummary> Products { get; set; }
    }

    public partial class SearchFreeTextResponseResult<T>
    {
        [JsonPropertyName("results")] public IEnumerable<T> Results { get; set; }
        [JsonPropertyName("totalCount")] public int TotalCount { get; set; }
    }
}