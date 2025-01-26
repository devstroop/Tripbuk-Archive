using System.Text.Json.Serialization;

namespace Tripbuk.Client.Viator;

public enum ItineraryType
{
    [JsonPropertyName("STANDARD")]
    Standard,
    [JsonPropertyName("ACTIVITY")]
    Activity,
    [JsonPropertyName("MULTI_DAY_TOUR")]
    MultiDayTour,
    [JsonPropertyName("HOP_ON_HOP_OFF")]
    HopOnHopOff,
    [JsonPropertyName("UNSTRUCTURED")]
    Unstructured
}