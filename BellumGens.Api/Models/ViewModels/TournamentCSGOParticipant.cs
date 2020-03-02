using System;
using System.Collections.Generic;

namespace BellumGens.Api.Models
{
    public class TournamentCSGOParticipant : TournamentParticipant
    {
        public TournamentCSGOParticipant(TournamentApplication application, List<TournamentCSGOMatch> matches)
            : base(application)
        {
            TeamId = application.TeamId;
            Team = new CSGOTeamSummaryViewModel(application.Team);
            TournamentCSGOGroupId = application.TournamentCSGOGroupId;
            if (matches != null)
            {
                foreach (TournamentCSGOMatch match in matches)
                {
                    if (match.Team1Id == Team.TeamId)
                        TeamPoints += match.Team1Points;
                    else if (match.Team2Id == Team.TeamId)
                        TeamPoints += match.Team2Points;
                }
            }
        }

        public Guid? TeamId { get; set; }
        public Guid? TournamentCSGOGroupId { get; set; }
        public int TeamPoints { get; set; } = 0;
        public CSGOTeamSummaryViewModel Team { get; set; }
    }
}