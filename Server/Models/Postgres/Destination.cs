using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tripbuk.Server.Models.Postgres
{
    [Table("Destinations", Schema = "public")]
    public partial class Destination
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        public int? ParentDestinationId { get; set; }

        public Destination Destination1 { get; set; }

        public string LookupId { get; set; }

        public string DestinationUrl { get; set; }

        public string DefaultCurrencyCode { get; set; }

        public string TimeZone { get; set; }

        [Column("IATACodes")]
        public List<string>? Iatacodes { get; set; }

        public string CountryCallingCode { get; set; }

        public List<string>? Languages { get; set; }

        public string Center { get; set; }

        public ICollection<Destination> Destinations1 { get; set; }
    }
}