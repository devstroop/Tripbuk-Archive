using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tripbuk.Server.Models.Viator
{
    public partial class Image
    {
        [JsonPropertyName("imageSource")]
        public string ImageSource { get; set; }

        [JsonPropertyName("isCover")]
        public bool IsCover { get; set; }

        [JsonPropertyName("variants")]
        public IEnumerable<Variant> Variants { get; set; }
    }
}