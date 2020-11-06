using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BellumGens.Api.Core.Models
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
                        _participants.Add(new TournamentCSGOParticipant(app, Matches.Where(m => m.Team1Id == app.TeamId || m.Team2Id == app.TeamId).ToList()));
                    }
                }
                return _participants.OrderByDescending(p => p.TeamPoints).ThenByDescending(p => p.RoundDifference).ToList();
            }
        }

        [JsonIgnore]
        public virtual ICollection<TournamentCSGOMatch> Matches { get; set; }
    }
}