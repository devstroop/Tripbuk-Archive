using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ERP.Server.Models.Postgres
{
    [Table("Items", Schema = "public")]
    public partial class Item
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
        public string ItemName { get; set; }

        [ConcurrencyCheck]
        public int? Group { get; set; }

        public ItemGroup ItemGroup { get; set; }

        [ConcurrencyCheck]
        public int? TenantId { get; set; }
    }
}