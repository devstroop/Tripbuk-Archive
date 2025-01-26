using System.Text.Json.Serialization;
using Tripbuk.Client.Viator;

namespace Tripbuk.Server.Models.Viator;

public abstract partial class DestinationDetails
{
    [JsonPropertyName("destinationId")] public int DestinationId { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("type")] public string Type { get; set; }
    [JsonPropertyName("parentDestinationId")] public int[] ParentDestinationId { get; set; }
    [JsonPropertyName("lookupId")] public string LookupId { get; set; }
    [JsonPropertyName("destinationUrl")] public string DestinationUrl { get; set; }
    [JsonPropertyName("defaultCurrencyCode")] public string DefaultCurrencyCode { get; set; }
    [JsonPropertyName("timeZone")] public string TimeZone { get; set; }
    [JsonPropertyName("iataCodes")] public string[] IataCodes { get; set; }
    [JsonPropertyName("countryCallingCode")] public string CountryCallingCode { get; set; }
    [JsonPropertyName("languages")] public string[] Languages { get; set; }
    [JsonPropertyName("center")] public LocationCenter Center { get; set; }
}