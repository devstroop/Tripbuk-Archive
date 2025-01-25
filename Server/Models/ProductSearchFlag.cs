using System.Text.Json.Serialization;

namespace Tripbuk.Server.Models;

public enum ProductSearchFlag
{
    [JsonPropertyName("NEW_ON_VIATOR")]
    NewOnViator,
    [JsonPropertyName("FEATURED")]
    SkipTheLine,
    [JsonPropertyName("SKIP_THE_LINE")]
    PrivateTour,
    [JsonPropertyName("PRIVATE_TOUR")]
    SpecialOffer,
    [JsonPropertyName("SPECIAL_OFFER")]
    LikelyToSellOut
}