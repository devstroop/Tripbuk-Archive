using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ERP.Server.Models.Postgres
{
    [Table("AccountGroupMasters", Schema = "public")]
    public partial class AccountGroupMaster
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
        public string GroupName { get; set; }

        [ConcurrencyCheck]
        public string Alias { get; set; }

        [ConcurrencyCheck]
        public bool? IsPrimary { get; set; }

        [ConcurrencyCheck]
        public int? Parent { get; set; }

        public AccountGroupMaster AccountGroupMaster1 { get; set; }

        public ICollection<AccountGroupMaster> AccountGroupMasters1 { get; set; }

        public ICollection<AccountMaster> AccountMasters { get; set; }
    }
}