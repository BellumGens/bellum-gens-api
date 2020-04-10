using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BellumGens.Api.Models
{
    public class JerseyOrder
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("email")]
        [EmailAddress]
        public string Email { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("streetAddress")]
        public string StreetAddress { get; set; }

        [JsonProperty("jerseys")]
        public virtual ICollection<JerseyDetails> Jerseys { get; set; } = new HashSet<JerseyDetails>();
    }
}