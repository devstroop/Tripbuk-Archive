using System.Text.Json.Serialization;

namespace Tripbuk.Client.Viator;

public abstract class ProductSearchPagination
{
    [JsonPropertyName("start")]
    public int Start { get; set; }
    [JsonPropertyName("count")]
    public int Count { get; set; }
}