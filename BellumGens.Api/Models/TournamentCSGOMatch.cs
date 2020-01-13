using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Models
{
    public class TournamentCSGOMatch : TournamentMatch
    {
        public Guid Team1Id { get; set; }
        public Guid Team2Id { get; set; }

        public virtual ICollection<CSGOMatchMap> Maps { get; set; }


        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Guid WinnerTeamId { get; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int Team1Points { get; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int Team2Points { get; }

        [ForeignKey("Team1Id")]
        public CSGOTeam Team1 { get; set; }
        [ForeignKey("Team2Id")]
        public CSGOTeam Team2 { get; set; }
    }
}