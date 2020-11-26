using BellumGens.Api.Models;
using BellumGens.Api.Providers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace BellumGens.Api.Controllers
{
	[Authorize]
    [RoutePrefix("api/Users")]
	public class UsersController : BaseController
    {
		//[Route("Users")]
		//[AllowAnonymous]
		//public async Task<UserStatsViewModel []> GetUsers(int page = 0)
		//{
		//	List<ApplicationUser> activeUsers = _dbContext.Users.OrderBy(e => e.Id).Skip(page * 10).Take(10).ToList();

		//	List<Task<UserStatsViewModel>> tasks = new List<Task<UserStatsViewModel>>();
		//	foreach (ApplicationUser user in activeUsers)
		//	{
		//		var model = new UserStatsViewModel(user);
		//		tasks.Add(model.GetSteamUserDetails());
		//	}

		//	return await Task.WhenAll(tasks).ConfigureAwait(false);
		//}

		[Route("User")]
		[AllowAnonymous]
		public async Task<IHttpActionResult> GetUserDetails(string userid)
		{
			UserStatsViewModel user = await SteamServiceProvider.GetSteamUserDetails(userid).ConfigureAwait(false);
            ApplicationUser registered = null;
            if (user.steamUser != null)
            {
                registered = _dbContext.Users.Include(u => u.MemberOf).FirstOrDefault(u => u.Id == user.steamUser.steamID64);
            }
			if (registered != null)
			{
				user.SetUser(registered);
			}
			return Ok(user);
		}

        [Route("UserGroups")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetUserGroups(string userid)
        {
            UserStatsViewModel user = await SteamServiceProvider.GetSteamUserDetails(userid).ConfigureAwait(false);
            return Ok(user?.steamUser?.groups);
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
            catch (DbUpdateException e)
			{
				System.Diagnostics.Trace.TraceError($"User availability error: ${e.Message}");
				return BadRequest("Something went wrong... ");
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
			catch (DbUpdateException e)
			{
				System.Diagnostics.Trace.TraceError($"User map pool error: ${e.Message}");
				return BadRequest("Something went wrong... ");
			}
			return Ok(userMap);
		}
		
		[Route("PrimaryRole")]
		[HttpPut]
		public IHttpActionResult SetPrimaryRole(Role role)
		{
			ApplicationUser user = GetAuthUser();
			_dbContext.Users.Find(user.Id).PreferredPrimaryRole = role.Id;
			try
			{
				_dbContext.SaveChanges();
			}
			catch (DbUpdateException e)
			{
				System.Diagnostics.Trace.TraceError($"User primary role error: ${e.Message}");
				return BadRequest("Something went wrong... ");
			}
			return Ok("success");
		}
		
		[Route("SecondaryRole")]
		[HttpPut]
		public IHttpActionResult SetSecondaryRole(Role role)
		{
			ApplicationUser user = GetAuthUser();
			_dbContext.Users.Find(user.Id).PreferredSecondaryRole = role.Id;
			try
			{
				_dbContext.SaveChanges();
			}
			catch (DbUpdateException e)
			{
				System.Diagnostics.Trace.TraceError($"User secondary role error: ${e.Message}");
				return BadRequest("Something went wrong... ");
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
				return NotFound();
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
			catch (DbUpdateException e)
			{
				System.Diagnostics.Trace.TraceError($"User team invite accept error: ${e.Message}");
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
				return NotFound();
			}

			entity.State = NotificationState.Rejected;
			try
			{
				_dbContext.SaveChanges();
			}
			catch (DbUpdateException e)
			{
				System.Diagnostics.Trace.TraceError($"User team invite reject error: ${e.Message}");
				return BadRequest("Something went wrong... ");
			}
			return Ok(entity);
		}
	}
}
