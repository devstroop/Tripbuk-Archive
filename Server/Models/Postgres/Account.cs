using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ERP.Server.Models.Postgres
{
    [Table("Accounts", Schema = "public")]
    public partial class Account
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
        public string AccountName { get; set; }

        [ConcurrencyCheck]
        public string Alias { get; set; }

        [ConcurrencyCheck]
        public string PrintName { get; set; }

        [ConcurrencyCheck]
        public int? Group { get; set; }

        public AccountGroup AccountGroup { get; set; }

        [ConcurrencyCheck]
        public decimal? CreditLimit { get; set; }

        [ConcurrencyCheck]
        public int? CreditDays { get; set; }

        [ConcurrencyCheck]
        public string Address { get; set; }

        [ConcurrencyCheck]
        public string City { get; set; }

        [ConcurrencyCheck]
        public string State { get; set; }

        [ConcurrencyCheck]
        public string PinCode { get; set; }

        [ConcurrencyCheck]
        public string Country { get; set; }

        [ConcurrencyCheck]
        public string Phone { get; set; }

        [ConcurrencyCheck]
        public string Mobile { get; set; }

        [ConcurrencyCheck]
        public string Whatsapp { get; set; }

        [ConcurrencyCheck]
        public string Email { get; set; }

        [ConcurrencyCheck]
        public string ContactPerson { get; set; }

        [Column("GSTIN")]
        [ConcurrencyCheck]
        public string Gstin { get; set; }

        [Column("PAN")]
        [ConcurrencyCheck]
        public string Pan { get; set; }

        [ConcurrencyCheck]
        public decimal? OpeningBalance { get; set; }

        [ConcurrencyCheck]
        public string OpeningBalanceType { get; set; }
    }
}