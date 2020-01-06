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
            UserId = application.UserId;
            BattleTag = application.BattleNetId;
            User = new UserInfoViewModel(application.User);
            State = application.State;
        }

        public string UserId { get; set; }
        public string BattleTag { get; set; }
        public UserInfoViewModel User { get; set; }
        public TournamentApplicationState State { get; set; }
    }
}