using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tripbuk.Server.Models.Postgres
{
    [Table("ParentTags", Schema = "public")]
    public partial class ParentTag
    {
        [Required]
        public int ParentTagId { get; set; }

        [Required]
        public int ChildTagId { get; set; }
    }
}