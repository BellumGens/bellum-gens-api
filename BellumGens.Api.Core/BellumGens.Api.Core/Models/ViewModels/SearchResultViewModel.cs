using System.Collections.Generic;

namespace BellumGens.Api.Core.Models
{
	public class SearchResultViewModel
	{
		public List<CSGOTeam> Teams { get; set; } = new List<CSGOTeam>();
		public List<CSGOStrategy> Strategies { get; set; } = new List<CSGOStrategy>();
        public List<UserStatsViewModel> Players { get; set; } = new List<UserStatsViewModel>();
        public UserStatsViewModel SteamUser { get; set; }
	}
}