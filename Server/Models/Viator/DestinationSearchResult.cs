using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tripbuk.Server.Models.Viator
{
    public partial class DestinationSearchResult
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("parentDestinationId")]
        public int ParentDestinationId { get; set; }

        [JsonPropertyName("parentDestinationName")]
        public string ParentDestinationName { get; set; }

        [JsonPropertyName("translationInfo")]
        public TranslationInfo TranslationInfo { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}