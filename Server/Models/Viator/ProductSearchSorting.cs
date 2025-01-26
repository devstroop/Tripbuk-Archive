using System.Text.Json.Serialization;

namespace Tripbuk.Client.Viator;

public partial class ProductSearchSorting 
{
    [JsonPropertyName("sort")]
    public string Sort { get; set; }
    [JsonPropertyName("order")]
    public string Order { get; set; }
}