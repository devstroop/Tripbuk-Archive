using System.Text.Json.Serialization;

namespace Tripbuk.Client.Viator;

public partial class LocationCenter
{
    [JsonPropertyName("Latitude")] public double Latitude { get; set; }
    [JsonPropertyName("Longitude")] public double Longitude { get; set; }
}