using System.Collections.Generic;
using System.Linq;

namespace BellumGens.Api.Core.Models
{
    // Models returned by AccountController actions.

    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
    }

    public class ManageInfoViewModel
    {
        public string LocalLoginProvider { get; set; }

        public string Email { get; set; }

        public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }

        public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
    }



    public class UserSummaryViewModel
    {
        protected ApplicationUser user;

        public UserSummaryViewModel() { }

        public UserSummaryViewModel(ApplicationUser user)
        {
            this.user = user;
        }

        public string id
        {
            get
            {
                return user?.Id;
            }
        }
        public string steamId
        {
            get
            {
                return user?.SteamID;
            }
        }
        public string username
        {
            get
            {
                return user?.UserName;
            }
        }
        public string avatarMedium
        {
            get
            {
                return user?.AvatarMedium;
            }
        }
        public string customURL
        {
            get
            {
                return user?.CustomUrl;
            }
        }
        public string battleNetId
        {
            get
            {
                return user?.BattleNetId;
            }
        }
    }

    public class UserInfoViewModel : UserSummaryViewModel
    {
        private List<CSGOTeamSummaryViewModel> _teams;
        private List<CSGOTeam> _teamAdmin;
        protected bool _isAuthUser;

		public UserInfoViewModel() : base() { }

        public UserInfoViewModel(ApplicationUser user, bool isAuthUser = false) : base(user)
        {
            _isAuthUser = isAuthUser;
        }

		public bool registered
		{
			get { return user != null; }
		}

        public List<CSGOTeamSummaryViewModel> teams
        {
            get
            {
                if (_teams == null && user != null)
                {
                    _teams = new List<CSGOTeamSummaryViewModel>();
                    foreach (TeamMember memberof in user.MemberOf)
                    {
                        _teams.Add(new CSGOTeamSummaryViewModel(memberof.Team));
                    }
                }
                return _teams;
            }
        }

        public List<CSGOTeam> teamAdmin
        {
            get
            {
                if (_isAuthUser && _teamAdmin == null && user != null)
                {
                    _teamAdmin = new List<CSGOTeam>();
                    foreach (TeamMember memberof in user.MemberOf)
                    {
                        if (memberof.IsAdmin)
							_teamAdmin.Add(memberof.Team);
                    }
                }
                return _teamAdmin;
            }
        }

        public ICollection<TeamInvite> notifications
        {
            get
            {
                return _isAuthUser ? user?.Notifications.OrderByDescending(n => n.Sent).ToList() : null;
            }
        }
        public List<string> externalLogins { get; set; }
        public bool? steamPrivate
        {
            get
            {
                return user?.SteamPrivate;
            }
        }
        public decimal? headshotPercentage
        {
            get
            {
                return user?.HeadshotPercentage;
            }
        }
        public decimal? killDeathRatio
        {
            get
            {
                return user?.KillDeathRatio;
            }
        }
        public decimal? accuracy
        {
            get
            {
                return user?.Accuracy;
            }
        }
        public string email
        {
            get
            {
                return _isAuthUser ? user?.Email : null;
            }
        }
        public bool? searchVisible
        {
            get
            {
                return user?.SearchVisible;
            }
        }

        public string avatarIcon
        {
            get
            {
                return user?.AvatarIcon;
            }
        }

        public string avatarFull
        {
            get
            {
                return user?.AvatarFull;
            }
        }

        public string realname
        {
            get
            {
                return user?.RealName;
            }
        }
        public string country
        {
            get
            {
                return user?.Country;
            }
        }
        public ICollection<UserAvailability> availability
        {
            get
            {
                return user?.Availability;
            }
        }
        public PlaystyleRole? primaryRole
        {
            get
            {
                return user?.PreferredPrimaryRole;
            }
        }
        public PlaystyleRole? secondaryRole
        {
            get
            {
                return user?.PreferredSecondaryRole;
            }
        }
        public ICollection<UserMapPool> mapPool
        {
            get
            {
                return user?.MapPool;
            }
        }
    }

    public class UserLoginInfoViewModel
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }
}
