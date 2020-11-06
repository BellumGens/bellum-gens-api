using System;
using System.Collections.Generic;
using System.Linq;

namespace BellumGens.Api.Core.Models
{
    public class TeamTournamentViewModel
    {
        private Tournament _tournament;
        private Guid _teamid;
        private List<TournamentCSGOMatch> _matches;
        public TeamTournamentViewModel(Tournament tournament, Guid teamid)
        {
            _tournament = tournament;
            _teamid = teamid;
        }

        public Guid ID
        {
            get
            {
                return _tournament.ID;
            }
        }
        public string Name
        {
            get
            {
                return _tournament.Name;
            }
        }
        public string Logo
        {
            get
            {
                return _tournament.Logo;
            }
        }
        public List<TournamentCSGOMatch> CSGOMatches
        {
            get
            {
                if (_matches == null)
                {
                    _matches = _tournament.CSGOMatches.Where(m => m.Team1Id == _teamid || m.Team2Id == _teamid).OrderByDescending(m => m.StartTime).ToList();
                }
                return _matches;
            }
        }
    }
}