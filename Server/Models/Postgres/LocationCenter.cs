using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tripbuk.Server.Models.Postgres
{
    [Table("LocationCenters", Schema = "public")]
    public partial class LocationCenter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public DateTime? CreatedAt { get; set; }

        public ICollection<Destination> Destinations { get; set; }
    }
}