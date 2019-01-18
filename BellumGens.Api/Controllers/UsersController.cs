using BellumGens.Api.Models;
using BellumGens.Api.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
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
			var registered = _dbContext.Users.Find(user.steamUser.steamID64);
			if (registered != null)
			{
				user.availability = registered.Availability;
				user.primaryRole = registered.PreferredPrimaryRole;
				user.secondaryRole = registered.PreferredSecondaryRole;
				user.mapPool = registered.MapPool;
			}
			return user;
		}

		[Route("Availability")]
		[HttpPut]
		public IHttpActionResult SetAvailability(UserAvailability newAvailability)
		{
			UserAvailability user = _dbContext.UserAvailabilities.Find(SteamServiceProvider.SteamUserId(User.Identity.GetUserId()), newAvailability.Day);
			newAvailability.UserId = user.UserId;
			_dbContext.Entry(user).CurrentValues.SetValues(newAvailability);
			try
			{
				_dbContext.SaveChanges();
			}
			catch
			{
				return BadRequest("Something went wrong...");
			}
			return Ok();
		}
		
		[Route("mapPool")]
		[HttpPut]
		public IHttpActionResult SetMapPool(UserMapPool mapPool)
		{
			UserMapPool userMap = _dbContext.UserMapPool.Find(SteamServiceProvider.SteamUserId(User.Identity.GetUserId()), mapPool.Map);
			mapPool.UserId = userMap.UserId;
			_dbContext.Entry(userMap).CurrentValues.SetValues(mapPool);
			try
			{
				_dbContext.SaveChanges();
			}
			catch
			{
				return BadRequest("Something went wrong...");
			}
			return Ok();
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
			return Ok();
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
			catch (Exception e)
			{
				return BadRequest("Something went wrong...");
			}
			return Ok();
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
				Member = user,
				IsActive = true,
                IsAdmin = false
			});
			entity.State = NotificationState.Accepted;
			try
			{
				_dbContext.SaveChanges();
			}
			catch (Exception e)
			{
				return BadRequest("Something went wrong...");
			}
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
			catch (Exception e)
			{
				return BadRequest("Something went wrong...");
			}
			return Ok(entity);
		}

		private ApplicationUser GetAuthUser()
		{
			return _dbContext.Users.Find(SteamServiceProvider.SteamUserId(User.Identity.GetUserId()));
		}
	}
}
