using System.Text.Json.Serialization;

namespace Tripbuk.Server.Models.Unsplash;

public partial class GetPhotoRequest
{
    [JsonPropertyName("query")] public string Query { get; set; }
    [JsonPropertyName("page")] public int Page { get; set; } = 1;
    [JsonPropertyName("perPage")] public int PerPage { get; set; } = 10;
    [JsonPropertyName("orderBy")] public string OrderBy { get; set; } = "relevant";
    [JsonPropertyName("orientation")] public string Orientation { get; set; } = "landscape";
}