﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Models
{
    public class TournamentCSGOMatch : TournamentMatch
    {
        public Guid WinnerTeamId { get; set; }

        public Guid Team1Id { get; set; }
        public Guid Team2Id { get; set; }

        public Uri DemoLink { get; set; }

        public Uri VideoLink { get; set; }
        public Tuple<CSGOTeam, CSGOTeam> Teams { get; set; }

        public ICollection<CSGOMatchMap> Maps { get; set; }

        [ForeignKey("Team1Id")]
        public CSGOTeam Team1 { get; set; }
        [ForeignKey("Team2Id")]
        public CSGOTeam Team2 { get; set; }
    }
}