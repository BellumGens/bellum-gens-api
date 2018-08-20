using SteamModels;
using SteamModels.CSGO;
using System.Collections.Generic;

namespace BellumGens.Api.Models
{
	public class UserStatsViewModel
	{
		public SteamUser steamUser;
		public CSGOPlayerStats userStats;
		public ICollection<UserAvailability> availability;
	}
}