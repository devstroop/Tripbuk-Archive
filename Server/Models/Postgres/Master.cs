using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ERP.Server.Models.Postgres
{
    [Table("Masters", Schema = "public")]
    public partial class Master
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
        public int Type { get; set; }

        [ConcurrencyCheck]
        public DateTime CreatedAt { get; set; }

        public ICollection<StdNarrationMaster> StdNarrationMasters { get; set; }

        public ICollection<ItemGroupMaster> ItemGroupMasters { get; set; }

        public ICollection<ItemMaster> ItemMasters { get; set; }

        public ICollection<AccountGroupMaster> AccountGroupMasters { get; set; }

        public ICollection<AccountMaster> AccountMasters { get; set; }
    }
}