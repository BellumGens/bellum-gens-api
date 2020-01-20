using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BellumGens.Api.Models
{
    public class TournamentSC2Participant
    {
        public TournamentSC2Participant(TournamentApplication application)
        {
            Id = application.Id;
            UserId = application.UserId;
            BattleTag = application.BattleNetId;
            User = new UserSummaryViewModel(application.User);
            State = application.State;
            TournamentSC2GroupId = application.TournamentSC2GroupId;
        }

        public Guid Id { get; set; }
        public Guid? TournamentSC2GroupId { get; set; }
        public string UserId { get; set; }
        public string BattleTag { get; set; }
        public UserSummaryViewModel User { get; set; }
        public TournamentApplicationState State { get; set; }
    }
}