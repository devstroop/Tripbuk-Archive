using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tripbuk.Server.Models.Postgres
{
    [Table("Tags", Schema = "public")]
    public partial class Tag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string TagName { get; set; }

        public string AllNamesByLocale { get; set; }
    }
}