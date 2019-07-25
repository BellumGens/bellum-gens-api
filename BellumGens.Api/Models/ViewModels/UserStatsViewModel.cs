using BellumGens.Api.Providers;
using SteamModels;
using SteamModels.CSGO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BellumGens.Api.Models
{
	public class UserStatsViewModel : UserInfoViewModel
	{
		public UserStatsViewModel() : base() { }

		public UserStatsViewModel(ApplicationUser user)
			: base(user) { }

		public bool steamUserException { get; set; } = false;
		public CSGOPlayerStats userStats { get; set; }
		public bool userStatsException { get; set; } = false;

		public async Task<UserStatsViewModel> GetSteamUserDetails()
		{
			UserStatsViewModel model = await SteamServiceProvider.GetSteamUserDetails(this.id);
			steamUser = model.steamUser;
			steamUserException = model.steamUserException;
			userStats = model.userStats;
			userStatsException = model.userStatsException;
			return this;
		}
	}
}