using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Models
{
    public class TournamentGroup
    {
        private List<TournamentCSGOParticipant> _participants;
        public TournamentGroup()
        {
            Participants = new HashSet<TournamentApplication>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid TournamentId { get; set; }

        [NotMapped]
        [JsonProperty("Participants")]
        public List<TournamentCSGOParticipant> PublicParticipants
        {
            get
            {
                if (_participants == null)
                {
                    _participants = new List<TournamentCSGOParticipant>();
                    foreach (TournamentApplication app in Participants)
                    {
                        _participants.Add(new TournamentCSGOParticipant(app));
                    }
                }
                return _participants;
            }
        }

        [ForeignKey("TournamentId")]
        [JsonIgnore]
        public virtual Tournament Tournament { get; set; }

        [JsonIgnore]
        public virtual ICollection<TournamentApplication> Participants { get; set; }
    }
}