using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BellumGens.Api.Core.Models
{
    public class JerseyOrder
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("email")]
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [JsonProperty("firstName")]
        [Required]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        [Required]
        public string LastName { get; set; }
        [JsonProperty("phoneNumber")]
        [Required]
        public string PhoneNumber { get; set; }
        [JsonProperty("city")]
        [Required]
        public string City { get; set; }
        [JsonProperty("streetAddress")]
        [Required]
        public string StreetAddress { get; set; }
        [JsonProperty("promocode")]
        public string PromoCode { get; set; }
        [JsonProperty("confirmed")]
        public bool Confirmed { get; set; } = false;
        [JsonProperty("shipped")]
        public bool Shipped { get; set; } = false;

        [JsonProperty("orderDate")]
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

        [ForeignKey("PromoCode")]
        public virtual Promo Promo { get; set; }

        [JsonProperty("jerseys")]
        public virtual ICollection<JerseyDetails> Jerseys { get; set; } = new HashSet<JerseyDetails>();
    }
}