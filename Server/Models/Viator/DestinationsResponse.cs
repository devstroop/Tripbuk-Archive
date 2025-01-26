using System.Text.Json.Serialization;

namespace Tripbuk.Server.Models.Viator;

public partial class DestinationsResponse
{
    [JsonPropertyName("destinations")] public IEnumerable<DestinationDetails> Destinations { get; set; }
    [JsonPropertyName("totalCount")] public int TotalCount { get; set; }
    // [JsonPropertyName("code")] public string Code { get; set; }
    // [JsonPropertyName("message")] public string Message { get; set; }
    // [JsonPropertyName("timestamp")] public DateTime Timestamp { get; set; }
    // [JsonPropertyName("trackingId")] public string TrackingId { get; set; }
}