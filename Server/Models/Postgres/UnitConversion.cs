using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TripBUK.Server.Models.Postgres
{
    [Table("UnitConversions", Schema = "public")]
    public partial class UnitConversion
    {

        [NotMapped]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("@odata.etag")]
        public string ETag
        {
            get;
            set;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int MainUnit { get; set; }

        public Unit Unit { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int SubUnit { get; set; }

        public Unit Unit1 { get; set; }

        [ConcurrencyCheck]
        public double ConversionFactor { get; set; }
    }
}