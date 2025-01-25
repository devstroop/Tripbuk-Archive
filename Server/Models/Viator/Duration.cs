using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tripbuk.Server.Models.Viator
{
    public partial class Duration
    {
        [JsonPropertyName("fixedDurationInMinutes")]
        public int FixedDurationInMinutes { get; set; }
    }
}