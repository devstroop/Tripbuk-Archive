using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tripbuk.Server.Models.Viator
{
    public partial class SearchFreeText
    {
        [JsonPropertyName("destinations")]
        public Destination Destinations { get; set; }

        [JsonPropertyName("attractions")]
        public Destination Attractions { get; set; }

        [JsonPropertyName("products")]
        public Destination Products { get; set; }
    }
}