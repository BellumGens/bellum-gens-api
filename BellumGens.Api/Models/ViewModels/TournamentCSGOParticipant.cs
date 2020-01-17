using System;

namespace BellumGens.Api.Models
{
    public class TournamentCSGOParticipant
    {
        public TournamentCSGOParticipant(TournamentApplication application)
        {
            Id = application.Id;
            UserId = application.UserId;
            Team = new CSGOTeamSummaryViewModel(application.Team);
            State = application.State;
            TournamentCSGOGroupId = application.TournamentCSGOGroupId;
        }

        public Guid Id { get; set; }
        public Guid? TournamentCSGOGroupId { get; set; }
        public string UserId { get; set; }
        public CSGOTeamSummaryViewModel Team { get; set; }
        public TournamentApplicationState State { get; set; }
    }
}