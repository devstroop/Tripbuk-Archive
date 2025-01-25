using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tripbuk.Server.Models.Postgres
{
    [Table("PlaceTags", Schema = "public")]
    public partial class PlaceTag
    {
        [Required]
        public Guid PlaceId { get; set; }

        [Required]
        public int TagId { get; set; }
    }
}