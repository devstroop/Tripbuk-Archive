using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tripbuk.Server.Models.Viator
{
    public partial class ProductSummary
    {
        [JsonPropertyName("productCode")]
        public int ProductCode { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("images")]
        public IEnumerable<Viator.Image> Images { get; set; }

        [JsonPropertyName("reviews")]
        public IEnumerable<Viator.ProductReviewsSummary> Reviews { get; set; }

        [JsonPropertyName("duration")]
        public ItineraryDuration Duration { get; set; }

        [JsonPropertyName("pricing")]
        public ProductSearchPricing Pricing { get; set; }

        [JsonPropertyName("productUrl")]
        public string ProductUrl { get; set; }

        [JsonPropertyName("destinations")]
        public IEnumerable<ProductSearchDestination> Destinations { get; set; }

        [JsonPropertyName("tags")]
        public IEnumerable<int> Tags { get; set; }

        [JsonPropertyName("flags")]
        public IEnumerable<string> Flags { get; set; }

        [JsonPropertyName("confirmationType")]
        public ProductConfirmationType ConfirmationType { get; set; }

        [JsonPropertyName("itineraryType")]
        public ProductItineraryType ItineraryType { get; set; }

        [JsonPropertyName("translationInfo")]
        public TranslationInfo TranslationInfo { get; set; }
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
        public Summary Summary { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("partnerNetFromPrice")]
        public double PartnerNetFromPrice { get; set; }
    }
    public partial class Summary
    {
        [JsonPropertyName("fromPrice")]
        public double FromPrice { get; set; }

        [JsonPropertyName("fromPriceBeforeDiscount")]
        public double FromPriceBeforeDiscount { get; set; }
    }
    public partial class ProductSearchDestination
    {
        [JsonPropertyName("ref")]
        public string Ref { get; set; }
        
        [JsonPropertyName("primary")]
        public bool Primary { get; set; }
    }

    public enum ProductConfirmationType
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        INSTANT,
        [JsonConverter(typeof(JsonStringEnumConverter))]
        MANUAL,
        [JsonConverter(typeof(JsonStringEnumConverter))]
        INSTANT_THEN_MANUAL
    }

    public enum ProductItineraryType
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        STANDARD,
        [JsonConverter(typeof(JsonStringEnumConverter))]
        ACTIVITY,
        [JsonConverter(typeof(JsonStringEnumConverter))]
        MULTI_DAY_TOUR,
        [JsonConverter(typeof(JsonStringEnumConverter))]
        HOP_ON_HOP_OFF,
        [JsonConverter(typeof(JsonStringEnumConverter))]
        UNSTRUCTURED
    }
}