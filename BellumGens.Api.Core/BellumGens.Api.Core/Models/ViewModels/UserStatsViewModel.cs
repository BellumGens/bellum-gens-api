using BellumGens.Api.Core.Providers;
using SteamModels;
using SteamModels.CSGO;
using System.Threading.Tasks;

namespace BellumGens.Api.Core.Models
{
	public class UserStatsViewModel : UserInfoViewModel
	{
		public UserStatsViewModel() : base() { }

		public UserStatsViewModel(ApplicationUser user, bool isAuthUser = false)
			: base(user, isAuthUser) { }

        public SteamUser steamUser { get; set; }
        public bool steamUserException { get; set; }
		public CSGOPlayerStats userStats { get; set; }
		public bool userStatsException { get; set; }

        public void SetUser(ApplicationUser user, BellumGensDbContext context)
        {
            this.user = user;
            RefreshAppUserValues(context);
        }

  //      public async Task<UserStatsViewModel> GetSteamUserDetails(BellumGensDbContext context)
		//{
		//	UserStatsViewModel model = await SteamServiceProvider.GetSteamUserDetails(id);
		//	steamUser = model.steamUser;
		//	steamUserException = model.steamUserException;
		//	userStats = model.userStats;
		//	userStatsException = model.userStatsException;
  //          RefreshAppUserValues(context);
		//	return this;
		//}

        private void RefreshAppUserValues(BellumGensDbContext context)
        {
            bool changes = false;
            if (steamUser?.avatarFull != user.AvatarFull)
            {
                user.AvatarFull = steamUser.avatarFull;
                changes = true;
            }
            if (steamUser?.steamID != user.UserName)
            {
                user.UserName = steamUser.steamID;
                changes = true;
            }
            if (steamUser?.avatarIcon != user.AvatarIcon)
            {
                user.AvatarIcon = steamUser.avatarIcon;
                changes = true;
            }
            if (steamUser?.realname != user.RealName)
            {
                user.RealName = steamUser.realname;
                changes = true;
            }
            if (steamUser?.avatarMedium != user.AvatarMedium)
            {
                user.AvatarMedium = steamUser.avatarMedium;
                changes = true;
            }
            if (steamUser?.customURL != user.CustomUrl)
            {
                user.CustomUrl = steamUser.customURL;
                changes = true;
            }
            if (steamUser?.country != user.Country)
            {
                user.Country = steamUser.country;
                changes = true;
            }
            if (userStatsException != user.SteamPrivate)
            {
                user.SteamPrivate = userStatsException;
                changes = true;
            }
            if (!userStatsException)
            {
                if (userStats?.headshotPercentage != user.HeadshotPercentage)
                {
                    user.HeadshotPercentage = userStats.headshotPercentage;
                    changes = true;
                }
                if (userStats?.killDeathRatio != user.KillDeathRatio)
                {
                    user.KillDeathRatio = userStats.killDeathRatio;
                    changes = true;
                }
                if (userStats?.accuracy != user.Accuracy)
                {
                    user.Accuracy = userStats.accuracy;
                    changes = true;
                }
            }
            if (changes)
            {
                try
                {
                    ApplicationUser appuser = context.Users.Find(user.Id);
                    context.Entry(appuser).CurrentValues.SetValues(user);
                    context.SaveChanges();
                }
                catch
                {

                }
            }
        }
	}
}