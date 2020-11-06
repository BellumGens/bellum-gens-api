using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Core.Models
{
    public class JerseyDetails
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid OrderId { get; set; }
        [JsonProperty("cut")]
        public JerseyCut Cut { get; set; }
        [JsonProperty("size")]
        public JerseySize Size { get; set; }

        [JsonIgnore]
        [ForeignKey("OrderId")]
        public virtual JerseyOrder Order { get; set; }
    }
}