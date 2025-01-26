using System.Text.Json.Serialization;
using Tripbuk.Client.Viator;

namespace Tripbuk.Server.Models;

public abstract class SearchFreetextRequest 
{
    [JsonPropertyName("searchTerm")]
    public required string SearchTerm { get; set; }
    [JsonPropertyName("productFiltering")]
    public ProductSearchFiltering Filtering { get; set; }
    [JsonPropertyName("productSorting")]
    public ProductSearchSorting Sorting { get; set; }
    [JsonPropertyName("searchTypes")]
    public required FreetextSearchType[] SearchTypes { get; set; }
    [JsonPropertyName("currency")]
    public required string Currency { get; set; }
}

public abstract class FreetextSearchType
{
    [JsonPropertyName("searchType")]
    public string SearchType { get; set; }
    [JsonPropertyName("pagination")]
    public ProductSearchPagination Pagination { get; set; }
}