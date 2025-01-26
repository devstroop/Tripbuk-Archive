using System.Text.Json.Serialization;

namespace Tripbuk.Client.Viator;

public partial class Destination
{
    [JsonPropertyName("ref")]
    public string Ref { get; set; }
        
    [JsonPropertyName("primary")]
    public bool Primary { get; set; }
}