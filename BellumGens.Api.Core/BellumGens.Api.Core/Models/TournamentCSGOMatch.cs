using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Core.Models
{
    public class TournamentCSGOMatch : TournamentMatch
    {
        public Guid Team1Id { get; set; }
        public Guid Team2Id { get; set; }
        public Guid? GroupId { get; set; }

        [NotMapped]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Guid WinnerTeamId { 
            get
            {
                return Team1Points == Team2Points ? Guid.Empty : Team1Points > Team2Points ? Team1Id : Team2Id;
            }
        }

        public int Team1Points { get; set; }

        public int Team2Points { get; set; }

        [NotMapped]
        [JsonProperty("Team1")]
        public CSGOTeamSummaryViewModel Team1Summary { 
            get
            {
                return Team1 != null ? new CSGOTeamSummaryViewModel(Team1) : null;
            }
        }

        [NotMapped]
        [JsonProperty("Team2")]
        public CSGOTeamSummaryViewModel Team2Summary
        {
            get
            {
                return Team2 != null ? new CSGOTeamSummaryViewModel(Team2) : null;
            }
        }

        public virtual ICollection<CSGOMatchMap> Maps { get; set; } = new HashSet<CSGOMatchMap>();

        [JsonIgnore]
        [ForeignKey("Team1Id")]
        public virtual CSGOTeam Team1 { get; set; }

        [JsonIgnore]
        [ForeignKey("Team2Id")]
        public virtual CSGOTeam Team2 { get; set; }

        [JsonIgnore]
        [ForeignKey("GroupId")]
        public virtual TournamentCSGOGroup Group { get; set; }
    }
}