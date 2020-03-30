using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BellumGens.Api.Models.ViewModels
{
    public class TeamTournamentsViewModel
    {
        public TeamTournamentsViewModel(List<Tournament> tournaments, Guid teamid)
        {

        }

        public List<Tournament> Tournaments { get; set; }
        public List<TournamentCSGOMatch> Matches { get; set; }
    }
}