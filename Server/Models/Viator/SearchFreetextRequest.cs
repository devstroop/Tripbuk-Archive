using System.Text.Json.Serialization;
namespace Tripbuk.Server.Models;

public abstract class SearchFreetextRequest 
{
    [JsonPropertyName("searchTerm")]
    public required string SearchTerm { get; set; }
    [JsonPropertyName("productFiltering")]
    public FreetextSearchProductFiltering ProductFiltering { get; set; }
    [JsonPropertyName("productSorting")]
    public FreetextSearchProductSorting ProductSorting { get; set; }
    [JsonPropertyName("searchTypes")]
    public required FreetextSearchType[] SearchTypes { get; set; }
    [JsonPropertyName("currency")]
    public required string Currency { get; set; }
}

public abstract class FreetextSearchProductFiltering 
{
    [JsonPropertyName("destination")]
    public string Destination { get; set; }
    [JsonPropertyName("dateRange")]
    public Range<DateTime> DateRange { get; set; }
    [JsonPropertyName("rating")]
    public Range<decimal> Rating { get; set; }
    [JsonPropertyName("durationInMinutes")]
    public Range<int> DurationInMinutes { get; set; }
    [JsonPropertyName("flags")]
    public ProductSearchFlag[] Flags { get; set; }
    [JsonPropertyName("includeAutomaticTranslations")]
    public bool IncludeAutomaticTranslations { get; set; } = true;
}

public abstract class FreetextSearchProductSorting 
{
    [JsonPropertyName("sort")]
    public string Sort { get; set; }
    [JsonPropertyName("order")]
    public string Order { get; set; }
}

public abstract class FreetextSearchType
{
    [JsonPropertyName("searchType")]
    public string SearchType { get; set; }
    [JsonPropertyName("pagination")]
    public FreetextSearchTypePagination Pagination { get; set; }
}

public abstract class FreetextSearchTypePagination
{
    [JsonPropertyName("start")]
    public int Start { get; set; }
    [JsonPropertyName("count")]
    public int Count { get; set; }
}