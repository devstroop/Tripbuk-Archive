using System.Text.Json.Serialization;

namespace Tripbuk.Server.Models.Viator;

public partial class ProductsSearchResponse
{
    [JsonPropertyName("products")] public IEnumerable<ProductSummary> Products { get; set; }
    [JsonPropertyName("totalCount")] public int TotalCount { get; set; }
}