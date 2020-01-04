namespace BellumGens.Api.Models
{
    public class TournamentCSGOParticipant
    {
        public TournamentCSGOParticipant(TournamentApplication application)
        {
            UserId = application.UserId;
            Team = new CSGOTeamSummaryViewModel(application.Team);
            State = application.State;
        }

        public string UserId { get; set; }
        public CSGOTeamSummaryViewModel Team { get; set; }
        public TournamentApplicationState State { get; set; }
    }
}