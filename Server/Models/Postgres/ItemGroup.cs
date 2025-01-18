using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ERP.Server.Models.Postgres
{
    [Table("ItemGroups", Schema = "public")]
    public partial class ItemGroup
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
        public string GroupName { get; set; }

        [ConcurrencyCheck]
        public string Alias { get; set; }

        [ConcurrencyCheck]
        public bool? IsPrimary { get; set; }

        [ConcurrencyCheck]
        public int? Parent { get; set; }

        public ItemGroup ItemGroup1 { get; set; }

        [ConcurrencyCheck]
        public int? TenantId { get; set; }

        public ICollection<ItemGroup> ItemGroups1 { get; set; }

        public ICollection<Item> Items { get; set; }
    }
}