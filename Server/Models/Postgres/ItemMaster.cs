using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ERP.Server.Models.Postgres
{
    [Table("ItemMasters", Schema = "public")]
    public partial class ItemMaster
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
        public string ItemName { get; set; }

        [ConcurrencyCheck]
        public int? Group { get; set; }

        public ItemGroupMaster ItemGroupMaster { get; set; }
    }
}