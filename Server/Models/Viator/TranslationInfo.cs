using System.Text.Json.Serialization;

namespace Tripbuk.Server.Models.Viator;

public partial class TranslationInfo
{
    [JsonPropertyName("containsMachineTranslatedText")]
    public bool ContainsMachineTranslatedText { get; set; }

    [JsonPropertyName("translationSource")]
    public string TranslationSource { get; set; }
}