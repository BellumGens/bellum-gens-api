using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Core.Models
{
    public class TournamentGroup
    {
        public TournamentGroup()
        {
            Participants = new HashSet<TournamentApplication>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid TournamentId { get; set; }

        [ForeignKey("TournamentId")]
        [JsonIgnore]
        public virtual Tournament Tournament { get; set; }

        [JsonIgnore]
        public virtual ICollection<TournamentApplication> Participants { get; set; }
    }
}