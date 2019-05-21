using BellumGens.Api.Models;
using BellumGens.Api.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Cookies;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BellumGens.Api.Controllers
{
	[EnableCors(origins: CORSConfig.allowedOrigins, headers: CORSConfig.allowedHeaders, methods: CORSConfig.allowedMethods, SupportsCredentials = true)]
	[Authorize]
    [HostAuthentication(CookieAuthenticationDefaults.AuthenticationType)]
    [RoutePrefix("api/Users")]
	public class UsersController : ApiController
    {
		private BellumGensDbContext _dbContext = new BellumGensDbContext();
        private ApplicationUserManager _userManager;

        [Route("Users")]
		[AllowAnonymous]
		public List<UserStatsViewModel> GetUsers(int page = 0)
		{
			List<UserStatsViewModel> steamUsers = new List<UserStatsViewModel>();
			List<string> activeUsers = _dbContext.Users.OrderBy(e => e.Id).Skip(page * 10).Take(10).Select(e => e.Id).ToList();

			foreach (string user in activeUsers)
			{
				steamUsers.Add(SteamServiceProvider.GetSteamUserDetails(user));
			}
			return steamUsers;
		}

		[Route("User")]
		[AllowAnonymous]
		public UserStatsViewModel GetUser(string userid)
		{
			UserStatsViewModel user = SteamServiceProvider.GetSteamUserDetails(userid);
			if (user.steamUser != null)
			{
				var registered = _dbContext.Users.Find(user.steamUser.steamID64);
				if (registered != null)
				{
                    UserInfoViewModel model = new UserInfoViewModel(registered);
					user.availability = model.availability;
					user.primaryRole = model.primaryRole;
					user.secondaryRole = model.secondaryRole;
					user.mapPool = model.mapPool;
					user.teams = model.teams;
					user.registered = true;
				}
			}
			return user;
		}

		[Route("Availability")]
		[HttpPut]
		public IHttpActionResult SetAvailability(UserAvailability newAvailability)
		{
			ApplicationUser user = GetAuthUser();
			UserAvailability entity = _dbContext.Users.Find(user.Id).Availability.First(a => a.Day == newAvailability.Day);
			_dbContext.Entry(entity).CurrentValues.SetValues(newAvailability);
			try
			{
				_dbContext.SaveChanges();
			}
			catch
			{
				return BadRequest("Something went wrong...");
			}
			return Ok(entity);
		}
		
		[Route("mapPool")]
		[HttpPut]
		public IHttpActionResult SetMapPool(UserMapPool mapPool)
		{
			ApplicationUser user = GetAuthUser();
			UserMapPool userMap = _dbContext.Users.Find(user.Id).MapPool.First(m => m.Map == mapPool.Map);
			_dbContext.Entry(userMap).CurrentValues.SetValues(mapPool);
			try
			{
				_dbContext.SaveChanges();
			}
			catch
			{
				return BadRequest("Something went wrong...");
			}
			return Ok(userMap);
		}
		
		[Route("PrimaryRole")]
		[HttpPut]
		public IHttpActionResult SetPrimaryRole(Role role)
		{
			ApplicationUser user = GetAuthUser();
			user.PreferredPrimaryRole = role.Id;
			try
			{
				_dbContext.SaveChanges();
			}
			catch
			{
				return BadRequest("Something went wrong...");
			}
			return Ok("success");
		}
		
		[Route("SecondaryRole")]
		[HttpPut]
		public IHttpActionResult SetSecondaryRole(Role role)
		{
			ApplicationUser user = GetAuthUser();
			user.PreferredSecondaryRole = role.Id;
			try
			{
				_dbContext.SaveChanges();
			}
			catch
			{
				return BadRequest("Something went wrong...");
			}
			return Ok("success");
		}

		[Route("AcceptTeamInvite")]
		[HttpPut]
		public IHttpActionResult AcceptTeamInvite(TeamInvite invite)
		{
			TeamInvite entity = _dbContext.TeamInvites.Find(invite.InvitingUserId, invite.InvitedUserId, invite.TeamId);
			if (entity == null)
			{
				return BadRequest("Invite couldn't be found");
			}

			ApplicationUser user = GetAuthUser();
			if (invite.InvitedUserId != user.Id)
			{
				return BadRequest("This invite was not sent to you...");
			}
			CSGOTeam team = _dbContext.Teams.Find(invite.TeamId);
			team.Members.Add(new TeamMember()
			{
				UserId = user.Id,
				IsActive = true,
                IsAdmin = false,
				IsEditor = false
			});
			entity.State = NotificationState.Accepted;
			try
			{
				_dbContext.SaveChanges();
			}
			catch
			{
				return BadRequest("Something went wrong...");
			}
			List<BellumGensPushSubscription> subs = _dbContext.PushSubscriptions.Where(s => s.userId == entity.InvitingUser.Id).ToList();
			NotificationsService.SendNotification(subs, entity, NotificationState.Accepted);
			return Ok(entity);
		}

		[Route("RejectTeamInvite")]
		[HttpPut]
		public IHttpActionResult RejectTeamInvite(TeamInvite invite)
		{
			TeamInvite entity = _dbContext.TeamInvites.Find(invite.InvitingUserId, invite.InvitedUserId, invite.TeamId);
			if (entity == null)
			{
				return BadRequest("Invite couldn't be found");
			}

			entity.State = NotificationState.Rejected;
			try
			{
				_dbContext.SaveChanges();
			}
			catch
			{
				return BadRequest("Something went wrong...");
			}
			return Ok(entity);
		}

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationUser GetAuthUser()
		{
			return UserManager.FindByName(User.Identity.GetUserName());
        }
	}
}
