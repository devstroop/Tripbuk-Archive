using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tripbuk.Server.Models.Postgres
{
    [Table("Places", Schema = "public")]
    public partial class Place
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Summary { get; set; }

        public string Description { get; set; }

        public string Contact { get; set; }

        public string Location { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string Center { get; set; }

        public List<string>? Images { get; set; }

        public ICollection<PlaceTag> PlaceTags { get; set; }
    }
}