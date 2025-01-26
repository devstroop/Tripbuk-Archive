using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tripbuk.Server.Models.Viator
{
    public partial class AttractionResult
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("parentDestinationId")]
        public int ParentDestinationId { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("destinationName")]
        public string DestinationName { get; set; }

        [JsonPropertyName("productsCount")]
        public int ParentDestinationName { get; set; }

        [JsonPropertyName("translationInfo")]
        public TranslationInfo TranslationInfo { get; set; }

        [JsonPropertyName("reviews")]
        public ProductReviewsSummary Reviews { get; set; }
        
        [JsonPropertyName("images")]
        public IEnumerable<string> Images { get; set; }
    }
}