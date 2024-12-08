using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ERP.Server.Models.Postgres
{
    [Table("StdNarrationMasters", Schema = "public")]
    public partial class StdNarrationMaster
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
        [Required]
        public int MasterId { get; set; }

        public Master Master { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int VoucherType { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string Narration { get; set; }
    }
}