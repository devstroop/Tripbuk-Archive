using System.Text.Json.Serialization;

namespace Tripbuk.Server.Models.Unsplash;

public partial class RandomPhotosResponse
{
    [JsonPropertyName("total")] public int Total { get; set; }
    [JsonPropertyName("results")] public IEnumerable<UnsplashPhoto> Results { get; set; }
}