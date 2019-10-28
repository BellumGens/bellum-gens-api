using BellumGens.Api.Providers;
using SteamModels;
using SteamModels.CSGO;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace BellumGens.Api.Models
{
	public class UserStatsViewModel : UserInfoViewModel
	{
		public UserStatsViewModel() : base() { }

		public UserStatsViewModel(ApplicationUser user)
			: base(user) { }

        public SteamUser steamUser { get; set; }
        public bool steamUserException { get; set; } = false;
		public CSGOPlayerStats userStats { get; set; }
		public bool userStatsException { get; set; } = false;

        public void SetUser(ApplicationUser user)
        {
            _user = user;
            RefreshAppUserValues();
        }

        public async Task<UserStatsViewModel> GetSteamUserDetails()
		{
			UserStatsViewModel model = await SteamServiceProvider.GetSteamUserDetails(this.id);
			steamUser = model.steamUser;
			steamUserException = model.steamUserException;
			userStats = model.userStats;
			userStatsException = model.userStatsException;
            RefreshAppUserValues();
			return this;
		}

        private void RefreshAppUserValues()
        {
            bool changes = false;
            if (steamUser?.avatarFull != _user.AvatarFull)
            {
                _user.AvatarFull = steamUser.avatarFull;
                changes = true;
            }
            if (steamUser?.steamID != _user.UserName)
            {
                _user.UserName = steamUser.steamID;
                changes = true;
            }
            if (steamUser?.avatarIcon != _user.AvatarIcon)
            {
                _user.AvatarIcon = steamUser.avatarIcon;
                changes = true;
            }
            if (steamUser?.realname != _user.RealName)
            {
                _user.RealName = steamUser.realname;
                changes = true;
            }
            if (steamUser?.avatarMedium != _user.AvatarMedium)
            {
                _user.AvatarMedium = steamUser.avatarMedium;
                changes = true;
            }
            if (steamUser?.customURL != _user.CustomUrl)
            {
                _user.CustomUrl = steamUser.customURL;
                changes = true;
            }
            if (steamUser?.country != _user.Country)
            {
                _user.Country = steamUser.country;
                changes = true;
            }
            if (changes)
            {
                using (BellumGensDbContext context = new BellumGensDbContext())
                {
                    try
                    {
                        ApplicationUser user = context.Users.Find(_user.Id);
                        context.Entry(user).CurrentValues.SetValues(_user);
                        context.SaveChanges();
                    }
                    catch
                    {

                    }
                }
            }
        }
	}
}