using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Models
{
    public class TournamentSC2Match : TournamentMatch
    {
        public string Player1Id { get; set; }
        public string Player2Id { get; set; }

        public Guid? GroupId { get; set; }

        public virtual ICollection<SC2MatchMap> Maps { get; set; }

        public int Player1Points { get; set; }

        public int Player2Points { get; set; }

        [ForeignKey("Player1Id")]
        public ApplicationUser Player1 { get; set; }
        [ForeignKey("Player2Id")]
        public ApplicationUser Player2 { get; set; }

        [JsonIgnore]
        [ForeignKey("GroupId")]
        public virtual TournamentSC2Group Group { get; set; }
    }
}