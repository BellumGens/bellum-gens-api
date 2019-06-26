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
        private ApplicationUser _user;

        public UserInfoViewModel(ApplicationUser user)
        {
            _user = user;
        }

		public bool registered
		{
			get { return true; }
		}

        public string id
        {
            get
            {
                return _user.Id;
            }
        }

        public SteamUser steamUser
        {
            get
            {
                return _user.SteamUser;
            }
        }

        public List<CSGOTeamSummaryViewModel> teams
        {
            get
            {
                if (_teams == null)
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
                if (_teamAdmin == null)
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
                return _user.Notifications.Where(n => n.State == NotificationState.NotSeen).ToList();
            }
        }
        public List<string> externalLogins { get; set; }
        public string email
        {
            get
            {
                return _user.Email;
            }
        }
        public bool searchVisible
        {
            get
            {
                return _user.SearchVisible;
            }
        }
        public ICollection<UserAvailability> availability
        {
            get
            {
                return _user.Availability;
            }
        }
        public PlaystyleRole primaryRole
        {
            get
            {
                return _user.PreferredPrimaryRole;
            }
        }
        public PlaystyleRole secondaryRole
        {
            get
            {
                return _user.PreferredSecondaryRole;
            }
        }
        public ICollection<UserMapPool> mapPool
        {
            get
            {
                return _user.MapPool;
            }
        }
    }

    public class UserLoginInfoViewModel
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }
}
