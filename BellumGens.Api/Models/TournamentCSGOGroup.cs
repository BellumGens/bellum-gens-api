using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Models
{
    public class TournamentCSGOGroup : TournamentGroup
    {
        private List<TournamentCSGOParticipant> _participants;

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

        [JsonIgnore]
        public virtual ICollection<TournamentCSGOMatch> Matches { get; set; }
    }
}