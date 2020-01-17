using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Models
{
    public class TournamentSC2Group : TournamentGroup
    {
        private List<TournamentSC2Participant> _participants;

        [NotMapped]
        [JsonProperty("Participants")]
        public List<TournamentSC2Participant> PublicParticipants
        {
            get
            {
                if (_participants == null)
                {
                    _participants = new List<TournamentSC2Participant>();
                    foreach (TournamentApplication app in Participants)
                    {
                        _participants.Add(new TournamentSC2Participant(app));
                    }
                }
                return _participants;
            }
        }

        [JsonIgnore]
        public virtual ICollection<TournamentSC2Match> Matches { get; set; }
    }
}