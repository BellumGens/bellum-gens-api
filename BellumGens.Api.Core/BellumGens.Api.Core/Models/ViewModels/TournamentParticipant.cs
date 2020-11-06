using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BellumGens.Api.Core.Models
{
    public class TournamentParticipant
    {
        public TournamentParticipant(TournamentApplication application)
        {
            Id = application.Id;
            UserId = application.UserId;
            State = application.State;
            Company = application.CompanyId;

        }

        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Company { get; set; }
        public int Matches { get; set; } = 0;
        public int Wins { get; set; } = 0;
        public int Losses { get; set; } = 0;
        public TournamentApplicationState State { get; set; }
    }
}