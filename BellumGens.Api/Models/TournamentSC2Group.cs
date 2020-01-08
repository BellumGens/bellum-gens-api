using System.Collections.Generic;

namespace BellumGens.Api.Models
{
    public class TournamentSC2Group : TournamentGroup
    {
        public virtual ICollection<TournamentSC2Match> Matches { get; }
    }
}