using System.Text.Json.Serialization;

namespace Tripbuk.Server.Viator;

public partial class TagWithAllLocalizations
{
    [JsonPropertyName("tagId")] public int TagId { get; set; }
    [JsonPropertyName("parentTagIds")] public IEnumerable<int> ParentTagIds { get; set; }
    [JsonPropertyName("allNamesByLocale")] public Dictionary<string, dynamic> AllNamesByLocale { get; set; }
}