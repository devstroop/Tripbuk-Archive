using System.Text.Json.Serialization;

namespace Tripbuk.Server.Models.Viator;

public abstract partial class DestinationsResponse
{
    [JsonPropertyName("destinations")] public IEnumerable<DestinationDetails> Destinations { get; set; }
    [JsonPropertyName("totalCount")] public int TotalCount { get; set; }
}