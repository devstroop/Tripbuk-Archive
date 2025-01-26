using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Tripbuk.Client.Viator;

namespace Tripbuk.Server.Models.Viator
{
    public partial class ProductSummary
    {
        [JsonPropertyName("productCode")] public int ProductCode { get; set; }
        [JsonPropertyName("title")] public string Title { get; set; }
        [JsonPropertyName("description")] public string Description { get; set; }
        [JsonPropertyName("images")] public IEnumerable<Viator.Image> Images { get; set; }
        [JsonPropertyName("reviews")] public IEnumerable<Viator.ProductReviewsSummary> Reviews { get; set; }
        [JsonPropertyName("duration")] public ItineraryDuration Duration { get; set; }
        [JsonPropertyName("pricing")] public ProductSearchPricing Pricing { get; set; }
        [JsonPropertyName("productUrl")] public string ProductUrl { get; set; }
        [JsonPropertyName("destinations")] public IEnumerable<Destination> Destinations { get; set; }
        [JsonPropertyName("tags")] public IEnumerable<int> Tags { get; set; }
        [JsonPropertyName("flags")] public IEnumerable<string> Flags { get; set; }
        [JsonPropertyName("confirmationType")] public ConfirmationType ConfirmationType { get; set; }
        [JsonPropertyName("itineraryType")] public ItineraryType ItineraryType { get; set; }
        [JsonPropertyName("translationInfo")] public TranslationInfo TranslationInfo { get; set; }
    }
    
    public partial class ItineraryDuration
    {
        [JsonPropertyName("fixedDurationInMinutes")]
        public int FixedDurationInMinutes { get; set; }
        
        [JsonPropertyName("variableDurationFromMinutes")]
        public int VariableDurationFromMinutes { get; set; }
        
        [JsonPropertyName("variableDurationToMinutes")]
        public int VariableDurationToMinutes { get; set; }
        
        [JsonPropertyName("unstructuredDuration")]
        public string UnstructuredDuration { get; set; }
    }
    public partial class ProductSearchPricing
    {
        [JsonPropertyName("summary")]
        public ProductSearchPricingSummary ProductSearchPricingSummary { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("partnerNetFromPrice")]
        public double PartnerNetFromPrice { get; set; }
    }
    public partial class ProductSearchPricingSummary
    {
        [JsonPropertyName("fromPrice")]
        public double FromPrice { get; set; }

        [JsonPropertyName("fromPriceBeforeDiscount")]
        public double FromPriceBeforeDiscount { get; set; }
    }

    
}