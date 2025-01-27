using System.Text.Json.Serialization;

namespace Tripbuk.Server.Models.Unsplash;

public partial class UnsplashPhoto
{
    [JsonPropertyName("id")] public string Id { get; set; }
    [JsonPropertyName("slug")] public string Slug { get; set; }
    [JsonPropertyName("alternative_slugs")] public Dictionary<string, string> AlternativeSlugs { get; set; }
    [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; }
    [JsonPropertyName("updated_at")] public DateTime? UpdatedAt { get; set; }
    [JsonPropertyName("description")] public string Description { get; set; }
    [JsonPropertyName("alt_description")] public string AltDescription { get; set; }
    [JsonPropertyName("urls")] public Dictionary<string, string> Urls { get; set; }
    [JsonPropertyName("likes")] public int Likes { get; set; }
    [JsonPropertyName("asset_type")] public string AssetType { get; set; }
    [JsonPropertyName("location")] public UnsplashPhotoLocation Location { get; set; }
    [JsonPropertyName("tags")] public IEnumerable<UnsplashPhotoTag> Tags { get; set; }
    [JsonPropertyName("views")] public int Views { get; set; }
    [JsonPropertyName("downloads")] public int Downloads { get; set; }
}

public partial class UnsplashPhotoTag
{
    [JsonPropertyName("type")] public string Type { get; set; }
    [JsonPropertyName("title")] public string Title { get; set; }
}
public partial class UnsplashPhotoLocation
{
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("city")] public string City { get; set; }
    [JsonPropertyName("country")] public string Country { get; set; }
    [JsonPropertyName("position")] public UnsplashPhotoLocationPosition Position { get; set; }
}

public partial class UnsplashPhotoLocationPosition
{
    [JsonPropertyName("latitude")] public double Latitude { get; set; }
    [JsonPropertyName("longitude")] public double Longitude { get; set; }
}