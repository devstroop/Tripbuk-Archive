using System.Text.Json.Serialization;

namespace Tripbuk.Client.Viator;

public enum ConfirmationType
{
    [JsonPropertyName("INSTANT")]
    Instant,
    [JsonPropertyName("MANUAL")]
    Manual,
    [JsonPropertyName("INSTANT_THEN_MANUAL")]
    InstantThenManual
}