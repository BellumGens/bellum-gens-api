using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Models
{
    public class SC2MatchMap
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public SC2Map Map { get; set; }

        public Guid SC2MatchId { get; set; }

        public string Winner { get; set; }

        [ForeignKey("SC2MatchId")]
        public virtual TournamentSC2Match Match { get; set; }
    }
}