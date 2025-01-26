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
        
        [JsonPropertyName("caption")]
        public string Caption { get; set; }

        [JsonPropertyName("isCover")]
        public bool IsCover { get; set; }

        [JsonPropertyName("variants")]
        public IEnumerable<ImageVariant> Variants { get; set; }
    }
    
    public partial class ImageVariant
    {
        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}