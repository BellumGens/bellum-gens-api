﻿using BellumGens.Api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using SteamModels;
using Microsoft.Owin.Security.Cookies;
using System;
using Microsoft.AspNet.Identity;
using BellumGens.Api.Providers;
using System.Net.Http;
using Microsoft.AspNet.Identity.Owin;

namespace BellumGens.Api.Controllers
{
	[EnableCors(origins: CORSConfig.allowedOrigins, headers: CORSConfig.allowedHeaders, methods: CORSConfig.allowedMethods, SupportsCredentials = true)]
	[Authorize]
	[HostAuthentication(CookieAuthenticationDefaults.AuthenticationType)]
	[RoutePrefix("api/Teams")]
	public class TeamsController : ApiController
	{
		private BellumGensDbContext _dbContext = new BellumGensDbContext();
        private ApplicationUserManager _userManager;

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

		[Route("Strats")]
		public IHttpActionResult GetTeamStrats(string teamId)
		{
			CSGOTeam team = UserIsTeamMember(teamId);
			if (team == null)
			{
				return BadRequest("You're not a member of this team.");
			}
			return Ok(team.Strategies);
		}

		[Route("Strat")]
		public IHttpActionResult GetTeamStrat(Guid stratId)
		{
			TeamStrategy strat = _dbContext.Strategies.Find(stratId);
			if (strat != null && strat.Team.Members.Any(m => m.UserId == GetAuthUser().Id))
			{
				return Ok(strat);
			}
			return BadRequest("Strat not found or user is not team member.");
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
		public IHttpActionResult GetSteamGroupMembers(string members)
		{
			List<SteamUserSummary> groupMembers = SteamServiceProvider.GetSteamUsersSummary(members);
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
			catch
			{
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
				catch
				{
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
			catch
			{
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
			catch
			{
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
			catch
			{
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
			if (team.Members.Count == 0)
			{
				_dbContext.Teams.Remove(team);
			}
			try
			{
				_dbContext.SaveChanges();
			}
			catch
			{
				return BadRequest("Something went wrong...");
			}
			return Ok("ok");
		}

		[Route("Invite")]
		[HttpPost]
		public IHttpActionResult InviteToTeam(string userId, Guid teamId)
		{
			CSGOTeam team = UserIsTeamAdmin(teamId);
			if (team == null)
			{
				return BadRequest("User is not team admin for " + team.TeamName);
			}

			ApplicationUser invitedUserEntity = _dbContext.Users.Find(userId);
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
			catch
			{
				return BadRequest("Something went wrong...");
			}
			List<BellumGensPushSubscription> subs = _dbContext.PushSubscriptions.Where(sub => sub.userId == invitedUserEntity.Id).ToList();
			NotificationsService.SendNotification(subs, invite);
			return Ok(userId);
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
				catch
				{
					return BadRequest("Something went wrong...");
				}
				List<TeamMember> admins = _dbContext.Teams.Find(application.TeamId).Members.Where(m => m.IsAdmin).ToList();
				List<BellumGensPushSubscription> subs = _dbContext.PushSubscriptions.Where(s => admins.Any(a => a.UserId == s.userId)).ToList();
				NotificationsService.SendNotification(subs, application);
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

			return Ok(team.Applications);
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
			catch
			{
				return BadRequest("Something went wrong...");
			}

			List<BellumGensPushSubscription> subs = _dbContext.PushSubscriptions.Where(s => s.userId == entity.ApplicantId).ToList();
			NotificationsService.SendNotification(subs, application, NotificationState.Accepted);
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
			catch
			{
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
			catch
			{
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
			catch
			{
				return BadRequest("Something went wrong...");
			}
			return Ok(entity);
		}

		[Route("Strategy")]
		[HttpPost]
		public IHttpActionResult SubmitStrategy(TeamStrategy strategy)
		{
			CSGOTeam team = UserIsTeamEditor(strategy.TeamId);
			if (team == null)
			{
				return BadRequest("You need to be team editor.");
			}

			TeamStrategy entity = team.Strategies.FirstOrDefault(s => s.Id == strategy.Id);
			if (entity == null)
			{
				entity = _dbContext.Strategies.Add(strategy);
			}
			else
			{
				_dbContext.Entry(entity).CurrentValues.SetValues(strategy);
			}

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

		[Route("Strategy")]
		[HttpDelete]
		public IHttpActionResult DeleteStrategy(Guid id, string teamid)
		{
			CSGOTeam team = UserIsTeamEditor(teamid);
			if (team == null)
			{
				return BadRequest("You need to be team editor.");
			}

			TeamStrategy entity = team.Strategies.FirstOrDefault(s => s.Id == id);
			if (entity != null)
			{
				_dbContext.Strategies.Remove(entity);
			}

			try
			{
				_dbContext.SaveChanges();
			}
			catch
			{
				return BadRequest("Something went wrong...");
			}
			return Ok("Ok");
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

        private CSGOTeam UserIsTeamAdmin(string teamId)
		{
			CSGOTeam team = ResolveTeam(teamId);
            ApplicationUser user = GetAuthUser();
            return team != null && team.Members.Any(m => m.IsAdmin && m.UserId == user.Id) ? team : null;
		}

		private CSGOTeam UserIsTeamAdmin(Guid teamId)
		{
			CSGOTeam team = _dbContext.Teams.Find(teamId);
			ApplicationUser user = GetAuthUser();
			return team != null && team.Members.Any(m => m.IsAdmin && m.UserId == user.Id) ? team : null;
		}

		private CSGOTeam UserIsTeamEditor(string teamId)
		{
			CSGOTeam team = ResolveTeam(teamId);
            ApplicationUser user = GetAuthUser();
            return team != null && team.Members.Any(m => m.IsEditor || m.IsAdmin && m.UserId == user.Id) ? team : null;
		}

		private CSGOTeam UserIsTeamEditor(Guid teamId)
		{
			CSGOTeam team = _dbContext.Teams.Find(teamId);
			ApplicationUser user = GetAuthUser();
			return team != null && team.Members.Any(m => m.IsEditor || m.IsAdmin && m.UserId == user.Id) ? team : null;
		}

		private CSGOTeam UserIsTeamMember(string teamId)
        {
            CSGOTeam team = ResolveTeam(teamId);
            ApplicationUser user = GetAuthUser();
            return team != null && team.Members.Any(m => m.UserId == user.Id) ? team : null;
        }

		private CSGOTeam UserIsTeamMember(Guid teamId)
		{
			CSGOTeam team = _dbContext.Teams.Find(teamId);
			ApplicationUser user = GetAuthUser();
			return team != null && team.Members.Any(m => m.UserId == user.Id) ? team : null;
		}

		private ApplicationUser GetAuthUser()
        {
            return UserManager.FindByName(User.Identity.GetUserName());
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
}
