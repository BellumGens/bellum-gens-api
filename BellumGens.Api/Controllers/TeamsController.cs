using BellumGens.Api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using SteamModels;
using System;
using BellumGens.Api.Providers;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace BellumGens.Api.Controllers
{
	[Authorize]
	[RoutePrefix("api/Teams")]
	public class TeamsController : BaseController
	{
        [Route("Teams")]
		[AllowAnonymous]
		public List<CSGOTeam> GetTeams()
		{
			return _dbContext.Teams.ToList();
		}

		[Route("Team")]
		[AllowAnonymous]
		public CSGOTeam GetTeam(string teamId)
		{
			return ResolveTeam(teamId);
		}

		[Route("teamadmin")]
		[HttpGet]
		public IHttpActionResult GetIsTeamAdmin(string teamid)
		{
			CSGOTeam team = UserIsTeamAdmin(teamid);
			if (team == null)
			{
				return Ok(false);
			}
			return Ok(true);
		}

		[Route("MapPool")]
		public IHttpActionResult GetTeamMapPool(string teamId)
		{
			CSGOTeam team = UserIsTeamMember(teamId);
			if (team == null)
			{
				return BadRequest("You're not a member of this team.");
			}
			return Ok(team.MapPool);
		}

		[Route("SteamMembers")]
		public async Task<IHttpActionResult> GetSteamGroupMembers(string members)
		{
			List<SteamUserSummary> groupMembers = await SteamServiceProvider.GetSteamUsersSummary(members).ConfigureAwait(false);
			return Ok(groupMembers);
		}

		[Route("Team")]
		[HttpPost]
		public IHttpActionResult TeamFromSteamGroup(SteamUserGroup group)
		{
            ApplicationUser user = GetAuthUser();
            if (!SteamServiceProvider.VerifyUserIsGroupAdmin(user.Id, group.groupID64))
			{
				return BadRequest("User is not a steam group owner for " + group.groupName);
			}

			CSGOTeam team = _dbContext.Teams.Add(new CSGOTeam()
			{
				SteamGroupId = group.groupID64,
				TeamName = group.groupName,
				TeamAvatar = group.avatarFull
			});
			team.InitializeDefaults();
			team.UniqueCustomUrl(_dbContext);

			team.Members.Add(new TeamMember()
			{
				UserId = user.Id,
				IsActive = true,
				IsAdmin = true,
				IsEditor = true
			});

			try
			{
				_dbContext.SaveChanges();
			}
			catch (DbUpdateException e)
			{
				System.Diagnostics.Trace.TraceWarning($"Attempting to register steam group twice: ${group.groupID64} msg: ${e.Message}");
				return BadRequest(group.groupName + " Steam group has already been registered.");
			}
			return Ok(team);
		}

		[Route("Team")]
		[HttpPut]
		public IHttpActionResult UpdateTeam(CSGOTeam team)
		{
			CSGOTeam entity = UserIsTeamAdmin(team.TeamId);
			if (entity == null)
			{
				return BadRequest("User is not a team admin for " + team.TeamName);
			}

			if (ModelState.IsValid)
			{
				_dbContext.Entry(entity).CurrentValues.SetValues(team);

				try
				{
					_dbContext.SaveChanges();
				}
				catch (DbUpdateException e)
				{
					System.Diagnostics.Trace.TraceError($"Team update error: ${e.Message}");
					return BadRequest("Something went wrong!");
				}
				return Ok(team);
			}
			return BadRequest("Invalid state of the team " + team.TeamName);
		}

		[Route("NewTeam")]
		[HttpPost]
		public IHttpActionResult NewTeam(CSGOTeam team)
		{
            ApplicationUser user = GetAuthUser();

			_dbContext.Teams.Add(team);

			team.Members.Add(new TeamMember()
			{
				UserId = user.Id,
				IsActive = true,
				IsAdmin = true,
				IsEditor = true
			});
			team.InitializeDefaults();
			team.UniqueCustomUrl(_dbContext);

			try
			{
				_dbContext.SaveChanges();
			}
			catch (DbUpdateException e)
			{
				System.Diagnostics.Trace.TraceError($"Team create error: ${e.Message}");
				return BadRequest("Something went wrong...");
			}
			return Ok(team);
		}

		[Route("Member")]
		[HttpPut]
		public IHttpActionResult UpdateTeamMember(TeamMember member)
		{
			CSGOTeam team = UserIsTeamAdmin(member.TeamId);
			if (team == null)
			{
				return BadRequest("User is not team admin...");
			}
			TeamMember entity = team.Members.SingleOrDefault(m => m.UserId == member.UserId);
			_dbContext.Entry(entity).CurrentValues.SetValues(member);
			try
			{
				_dbContext.SaveChanges();
			}
			catch (DbUpdateException e)
			{
				System.Diagnostics.Trace.TraceError($"Team member update error: ${e.Message}");
				return BadRequest("Something went wrong...");
			}
			return Ok("ok");
		}

		[Route("RemoveMember")]
		[HttpDelete]
		public IHttpActionResult RemoveTeamMember(Guid teamId, string userId)
		{
			CSGOTeam team = UserIsTeamAdmin(teamId);
			if (team == null)
			{
				return BadRequest("User is not team admin...");
			}
			TeamMember entity = team.Members.SingleOrDefault(m => m.UserId == userId);
			_dbContext.TeamMembers.Remove(entity);
			try
			{
				_dbContext.SaveChanges();
			}
			catch (DbUpdateException e)
			{
				System.Diagnostics.Trace.TraceError($"Team member delete error: ${e.Message}");
				return BadRequest("Something went wrong...");
			}
			return Ok("ok");
		}

		[Route("Abandon")]
		[HttpDelete]
		public IHttpActionResult AbandonTeam(Guid teamId)
		{
            ApplicationUser user = GetAuthUser();
			CSGOTeam team = _dbContext.Teams.Find(teamId);
			TeamMember entity = team.Members.SingleOrDefault(e => e.UserId == user.Id);
			team.Members.Remove(entity);
			object response = new { removed = false };
			if (team.Members.Count == 0)
			{
				_dbContext.Teams.Remove(team);
				response = new { removed = true };
			}
			else if (!team.Members.Any(m => m.IsAdmin))
			{
				team.Members.First().IsAdmin = true;
			}

			try
			{
				_dbContext.SaveChanges();
			}
			catch (DbUpdateException e)
			{
				System.Diagnostics.Trace.TraceError($"Team abandon error: ${e.Message}");
				return BadRequest("Could not remove team because there's an active tournament registration associated with it!");
			}
			return Ok(response);
		}

		[Route("Invite")]
		[HttpPost]
		public IHttpActionResult InviteToTeam(InviteModel model)
		{
			CSGOTeam team = UserIsTeamAdmin(model.teamId);
			if (team == null)
			{
				return BadRequest("User is not team admin for " + team.TeamName);
			}

			ApplicationUser invitedUserEntity = _dbContext.Users.Find(model.userId);
			ApplicationUser invitingUserEntity = GetAuthUser();
			TeamInvite invite = team.Invites.SingleOrDefault(i => i.InvitingUserId == invitingUserEntity.Id && i.InvitedUserId == invitedUserEntity.Id);
			
			if (invite != null)
			{
				if (invite.State != NotificationState.NotSeen)
				{
					invite.State = NotificationState.NotSeen;
				}
			}
			else
			{
				invite = new TeamInvite()
				{
					InvitedUser = invitedUserEntity,
					InvitingUserId = invitingUserEntity.Id
				};
				team.Invites.Add(invite);
			}
			try
			{
				_dbContext.SaveChanges();
			}
			catch (DbUpdateException e)
			{
				System.Diagnostics.Trace.TraceError($"Team invite error: ${e.Message}");
				return BadRequest("Something went wrong...");
			}
			List<BellumGensPushSubscription> subs = _dbContext.PushSubscriptions.Where(sub => sub.userId == invitedUserEntity.Id).ToList();
			NotificationsService.SendNotification(subs, invite);
			return Ok(model.userId);
		}

		[Route("Apply")]
		[HttpPost]
		public IHttpActionResult ApplyToTeam(TeamApplication application)
		{
			if (ModelState.IsValid)
			{
				TeamApplication entity = _dbContext.TeamApplications.Find(application.ApplicantId, application.TeamId);
				if (entity != null)
				{
					entity.Message = application.Message;
					entity.Sent = DateTimeOffset.Now;
					entity.State = NotificationState.NotSeen;
				}
				else
				{
					_dbContext.TeamApplications.Add(application);
				}

				try
				{
					_dbContext.SaveChanges();
				}
				catch (DbUpdateException e)
				{
					System.Diagnostics.Trace.TraceError($"Team application error: ${e.Message}");
					return BadRequest("Something went wrong...");
				}
				List<TeamMember> admins = _dbContext.Teams.Find(application.TeamId).Members.Where(m => m.IsAdmin).ToList();
				try
				{
					List<BellumGensPushSubscription> subs = _dbContext.PushSubscriptions.ToList();
					subs = subs.FindAll(s => admins.Any(a => a.UserId == s.userId));
					NotificationsService.SendNotification(subs, application);
				}
				catch (Exception e)
				{
					System.Diagnostics.Trace.TraceError($"Push sub error: ${e.Message}");
				}
				return Ok(application);
			}
			return BadRequest("Something went wrong with your application validation");
		}

		[Route("Applications")]
		public IHttpActionResult GetTeamApplications(Guid teamId)
		{
			CSGOTeam team = UserIsTeamAdmin(teamId);
			if (team == null)
			{
				return BadRequest("You need to be team admin.");
			}

			return Ok(team.Applications.Where(a => a.State == NotificationState.NotSeen).ToList());
		}

		[Route("ApproveApplication")]
		[HttpPut]
		public IHttpActionResult ApproveApplication(TeamApplication application)
		{
			CSGOTeam team = UserIsTeamAdmin(application.TeamId);
			if (team == null)
			{
				return BadRequest("You need to be team admin.");
			}

			TeamApplication entity = team.Applications.SingleOrDefault(a => a.ApplicantId == application.ApplicantId);
			entity.State = NotificationState.Accepted;

			ApplicationUser user = _dbContext.Users.Find(application.ApplicantId);
			team.Members.Add(new TeamMember()
			{
				Member = user,
				IsActive = true,
				IsAdmin = false,
				IsEditor = false
			});
			try
			{
				_dbContext.SaveChanges();
			}
			catch (DbUpdateException e)
			{
				System.Diagnostics.Trace.TraceError($"Team application approve error: ${e.Message}");
				return BadRequest("Something went wrong...");
			}

			try
			{
				List<BellumGensPushSubscription> subs = _dbContext.PushSubscriptions.Where(s => s.userId == entity.ApplicantId).ToList();
				NotificationsService.SendNotification(subs, application, NotificationState.Accepted);
			}
			catch (Exception e)
			{
				System.Diagnostics.Trace.TraceWarning($"Team application approve push notification fail: ${e.Message}");
			}

			return Ok(entity);
		}

		[Route("RejectApplication")]
		[HttpPut]
		public IHttpActionResult RejectApplication(TeamApplication application)
		{
			CSGOTeam team = UserIsTeamAdmin(application.TeamId);
			if (team == null)
			{
				return BadRequest("You need to be team admin.");
			}

			TeamApplication entity = team.Applications.SingleOrDefault(a => a.ApplicantId == application.ApplicantId);
			entity.State = NotificationState.Rejected;
			try
			{
				_dbContext.SaveChanges();
			}
			catch (DbUpdateException e)
			{
				System.Diagnostics.Trace.TraceError($"Team application reject error: ${e.Message}");
				return BadRequest("Something went wrong... ");
			}
			return Ok("ok");
		}

		[Route("MapPool")]
		[HttpPut]
		public IHttpActionResult SetTeamMapPool(List<TeamMapPool> maps)
		{
			CSGOTeam team = UserIsTeamAdmin(maps[0].TeamId);
			if (team == null)
			{
				return BadRequest("You need to be team admin.");
			}

			foreach (TeamMapPool mapPool in maps)
			{
				TeamMapPool entity = team.MapPool.SingleOrDefault(m => m.Map == mapPool.Map);
				_dbContext.Entry(entity).CurrentValues.SetValues(mapPool);
			}
			try
			{
				_dbContext.SaveChanges();
			}
			catch (DbUpdateException e)
			{
				System.Diagnostics.Trace.TraceError($"Team map pool error: ${e.Message}");
				return BadRequest("Something went wrong...");
			}
			return Ok("ok");
		}

		[Route("availability")]
		[HttpPut]
		public IHttpActionResult SetTeamAvailability(TeamAvailability day)
		{
			CSGOTeam team = UserIsTeamAdmin(day.TeamId);
			if (team == null)
			{
				return BadRequest("You need to be team admin.");
			}

			TeamAvailability entity = team.PracticeSchedule.FirstOrDefault(s => s.Day == day.Day);
			_dbContext.Entry(entity).CurrentValues.SetValues(day);
			try
			{
				_dbContext.SaveChanges();
			}
			catch (DbUpdateException e)
			{
				System.Diagnostics.Trace.TraceError($"Team availability error: ${e.Message}");
				return BadRequest("Something went wrong...");
			}
			return Ok(entity);
		}

		private CSGOTeam UserIsTeamAdmin(Guid teamId)
		{
			CSGOTeam team = _dbContext.Teams.Find(teamId);
			ApplicationUser user = GetAuthUser();
			return team != null && team.Members.Any(m => m.IsAdmin && m.UserId == user.Id) ? team : null;
		}

		private CSGOTeam UserIsTeamAdmin(string teamId)
		{
			CSGOTeam team = ResolveTeam(teamId);
			ApplicationUser user = GetAuthUser();
			return team != null && team.Members.Any(m => m.IsAdmin && m.UserId == user.Id) ? team : null;
		}

		private CSGOTeam UserIsTeamMember(string teamId)
        {
            CSGOTeam team = ResolveTeam(teamId);
            ApplicationUser user = GetAuthUser();
            return team != null && team.Members.Any(m => m.UserId == user.Id) ? team : null;
        }

		private CSGOTeam ResolveTeam(string teamId)
		{
			CSGOTeam team = _dbContext.Teams.FirstOrDefault(t => t.CustomUrl == teamId);
			if (team == null)
			{
				var valid = Guid.TryParse(teamId, out Guid id);
				if (valid)
				{
					team = _dbContext.Teams.Find(id);
				}
			}
			return team;
		}
	}

	public class InviteModel
	{
		public string userId { get; set; }
		public Guid teamId { get; set; }
	}
}
