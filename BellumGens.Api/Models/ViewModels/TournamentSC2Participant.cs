using System;
using System.Collections.Generic;
using System.Linq;

namespace BellumGens.Api.Models
{
    public class TournamentSC2Participant
    {
        public TournamentSC2Participant(TournamentApplication application, List<TournamentSC2Match> matches)
        {
            Id = application.Id;
            UserId = application.UserId;
            BattleTag = application.BattleNetId;
            User = new UserSummaryViewModel(application.User);
            State = application.State;
            TournamentSC2GroupId = application.TournamentSC2GroupId;
            if (matches != null)
            {
                foreach (TournamentSC2Match match in matches)
                {
                    if (match.Player1Id == UserId)
                        PlayerPoints += match.Player1Points;
                    else if (match.Player2Id == UserId)
                        PlayerPoints += match.Player2Points;
                }
            }
        }

        public Guid Id { get; set; }
        public Guid? TournamentSC2GroupId { get; set; }
        public string UserId { get; set; }
        public string BattleTag { get; set; }
        public int PlayerPoints { get; set; } = 0;
        public UserSummaryViewModel User { get; set; }
        public TournamentApplicationState State { get; set; }
    }
}