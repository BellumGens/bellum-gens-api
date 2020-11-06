using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BellumGens.Api.Core.Models
{
    public class RegistrationCountViewModel
    {
        public RegistrationCountViewModel(List<TournamentApplication> registrations, Game gameType)
        {
            game = gameType;
            count = registrations.Where(r => r.Game == game).Count();
        }
        public Game game { get; set; }
        public int count { get; set; }
    }
}