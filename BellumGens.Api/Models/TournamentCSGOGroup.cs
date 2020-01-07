using System.Collections.Generic;

namespace BellumGens.Api.Models
{
    public class TournamentCSGOGroup : TournamentGroup
    {
        public virtual ICollection<TournamentApplication> Participants { get; }

        public virtual ICollection<TournamentCSGOMatch> Matches { get; }
    }
}