using SteamModels;
using SteamModels.CSGO;
using System.Collections.Generic;

namespace BellumGens.Api.Models
{
	public class UserStatsViewModel
	{
		public SteamUser steamUser;
		public string steamUserException;
		public CSGOPlayerStats userStats;
		public string userStatsException;
		public ICollection<UserAvailability> availability;
		public PlaystyleRole primaryRole;
		public PlaystyleRole secondaryRole;
		public ICollection<UserMapPool> mapPool;
		public ICollection<CSGOTeamSummaryViewModel> teams;
		public bool registered = false;
	}
}