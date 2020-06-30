using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Models
{
    public class Promo
    {
        [Key]
        [Index(IsUnique = true)]
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("discount")]
        public decimal Discount { get; set; }

        [JsonProperty("expiration")]
        public DateTimeOffset? Expiration { get; set; }
    }
}