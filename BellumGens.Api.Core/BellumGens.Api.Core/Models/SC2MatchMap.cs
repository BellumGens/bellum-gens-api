using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Core.Models
{
    public class SC2MatchMap
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public SC2Map Map { get; set; }

        public Guid SC2MatchId { get; set; }

        public string PlayerPickId { get; set; }

        public string PlayerBanId { get; set; }

        public string WinnerId { get; set; }

        [JsonIgnore]
        [ForeignKey("SC2MatchId")]
        public virtual TournamentSC2Match Match { get; set; }

        [JsonIgnore]
        [ForeignKey("PlayerPickId")]
        public virtual ApplicationUser PickingPlayer { get; set; }

        [JsonIgnore]
        [ForeignKey("PlayerBanId")]
        public virtual ApplicationUser BanningPlayer { get; set; }
    }
}