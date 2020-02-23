using BellumGens.Api.Providers;
using SteamModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BellumGens.Api.Models
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

    public class UserInfoViewModel
    {
        private List<CSGOTeamSummaryViewModel> _teams;
        private List<CSGOTeam> _teamAdmin;
        protected ApplicationUser _user;
        protected bool _isAuthUser;

		public UserInfoViewModel() { }

        public UserInfoViewModel(ApplicationUser user, bool isAuthUser = false)
        {
            _user = user;
            _isAuthUser = isAuthUser;
        }

		public bool registered
		{
			get { return _user != null; }
		}

        public string id
        {
            get
            {
                return _user?.Id;
            }
        }

        public List<CSGOTeamSummaryViewModel> teams
        {
            get
            {
                if (_teams == null && _user != null)
                {
                    _teams = new List<CSGOTeamSummaryViewModel>();
                    foreach (TeamMember memberof in _user.MemberOf)
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
                if (_isAuthUser && _teamAdmin == null && _user != null)
                {
                    _teamAdmin = new List<CSGOTeam>();
                    foreach (TeamMember memberof in _user.MemberOf)
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
                return _isAuthUser ? _user?.Notifications.OrderByDescending(n => n.Sent).ToList() : null;
            }
        }
        public List<string> externalLogins { get; set; }

        public bool? steamPrivate
        {
            get
            {
                return _user?.SteamPrivate;
            }
        }
        public decimal? headshotPercentage
        {
            get
            {
                return _user?.HeadshotPercentage;
            }
        }
        public decimal? killDeathRatio
        {
            get
            {
                return _user?.KillDeathRatio;
            }
        }
        public decimal? accuracy
        {
            get
            {
                return _user?.Accuracy;
            }
        }
        public string email
        {
            get
            {
                return _isAuthUser ? _user?.Email : null;
            }
        }
        public bool? searchVisible
        {
            get
            {
                return _user?.SearchVisible;
            }
        }

        public string avatarIcon
        {
            get
            {
                return _user?.AvatarIcon;
            }
        }

        public string avatarMedium
        {
            get
            {
                return _user?.AvatarMedium;
            }
        }

        public string avatarFull
        {
            get
            {
                return _user?.AvatarFull;
            }
        }

        public string username
        {
            get
            {
                return _user?.UserName;
            }
        }

        public string realname
        {
            get
            {
                return _user?.RealName;
            }
        }
        public string customURL
        {
            get
            {
                return _user?.CustomUrl;
            }
        }
        public string country
        {
            get
            {
                return _user?.Country;
            }
        }
        public ICollection<UserAvailability> availability
        {
            get
            {
                return _user?.Availability;
            }
        }
        public PlaystyleRole? primaryRole
        {
            get
            {
                return _user?.PreferredPrimaryRole;
            }
        }
        public PlaystyleRole? secondaryRole
        {
            get
            {
                return _user?.PreferredSecondaryRole;
            }
        }
        public ICollection<UserMapPool> mapPool
        {
            get
            {
                return _user?.MapPool;
            }
        }
    }

    public class UserSummaryViewModel
    {
        private ApplicationUser user;

        public UserSummaryViewModel(ApplicationUser user)
        {
            this.user = user;
        }

        public string id
        {
            get
            {
                return user.Id;
            }
        }
        public string username
        {
            get
            {
                return user.UserName;
            }
        }
        public string avatarMedium
        {
            get
            {
                return user.AvatarMedium;
            }
        }
        public string customUrl
        {
            get
            {
                return user.CustomUrl;
            }
        }
    }

    public class UserLoginInfoViewModel
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }
}
