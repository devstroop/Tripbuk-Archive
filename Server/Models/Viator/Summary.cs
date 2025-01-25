using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tripbuk.Server.Models.Viator
{
    public partial class Summary
    {
        [JsonPropertyName("fromPrice")]
        public double FromPrice { get; set; }

        [JsonPropertyName("fromPriceBeforeDiscount")]
        public double FromPriceBeforeDiscount { get; set; }
    }
}