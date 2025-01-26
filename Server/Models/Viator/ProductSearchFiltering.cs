using System.Text.Json.Serialization;
using Tripbuk.Server.Models;

namespace Tripbuk.Client.Viator;

public partial class ProductSearchFiltering 
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