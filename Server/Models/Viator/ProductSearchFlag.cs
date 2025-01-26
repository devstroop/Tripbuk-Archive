using System.Text.Json.Serialization;

namespace Tripbuk.Client.Viator;

public enum ProductSearchFlag
{
    [JsonPropertyName("NEW_ON_VIATOR")] NewOnViator,
    [JsonPropertyName("FREE_CANCELLATION")] FreeCancellation,
    [JsonPropertyName("ALL_SALES_FINAL")] AllSalesFinal,
    [JsonPropertyName("SKIP_THE_LINE")] SkipTheLine,
    [JsonPropertyName("PRIVATE_TOUR")] PrivateTour,
    [JsonPropertyName("SPECIAL_OFFER")] SpecialOffer,
    [JsonPropertyName("LIKELY_TO_SELL_OUT")] LikelyToSellOut
}