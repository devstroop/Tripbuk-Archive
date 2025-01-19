using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ERP.Server.Models.Postgres
{
    [Table("SmtpConfigs", Schema = "public")]
    public partial class SmtpConfig
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
        [Column("id")]
        [Required]
        public Guid Id { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string Host { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int Port { get; set; }

        [ConcurrencyCheck]
        public bool? Ssl { get; set; }

        [ConcurrencyCheck]
        public string User { get; set; }

        [ConcurrencyCheck]
        public string Password { get; set; }
    }
}