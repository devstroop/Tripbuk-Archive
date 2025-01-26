using System.Text.Json.Serialization;
using Tripbuk.Client.Viator;

namespace Tripbuk.Server.Models.Viator;

public partial class ProductsSearchRequest
{
    [JsonPropertyName("filtering")] public ProductSearchFiltering Filtering { get; set; }
    [JsonPropertyName("sorting")] public ProductSearchSorting Sorting { get; set; }
    [JsonPropertyName("pagination")] public ProductSearchPagination Pagination { get; set; }
    [JsonPropertyName("currency")] public required string Currency { get; set; }
}