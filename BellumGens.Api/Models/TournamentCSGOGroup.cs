using Newtonsoft.Json;
using System.Collections.Generic;

namespace BellumGens.Api.Models
{
    public class TournamentCSGOGroup : TournamentGroup
    {
        [JsonIgnore]
        public virtual ICollection<TournamentCSGOMatch> Matches { get; set; }
    }
}