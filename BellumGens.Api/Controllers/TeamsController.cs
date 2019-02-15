using BellumGens.Api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using SteamModels;
using Microsoft.Owin.Security.Cookies;
using System;
using Microsoft.AspNet.Identity;
using BellumGens.Api.Providers;

namespace BellumGens.Api.Controllers
{
    [EnableCors(origins: CORSConfig.allowedOrigins, headers: CORSConfig.allowedHeaders, methods: CORSConfig.allowedMethods, SupportsCredentials = true)]
    [Authorize]
    [HostAuthentication(CookieAuthenticationDefaults.AuthenticationType)]
    [RoutePrefix("api/Teams")]
	public class TeamsController : ApiController
    {
		private BellumGensDbContext _dbContext = new BellumGensDbContext();

		[Route("Teams")]
		[AllowAnonymous]
		public List<CSGOTeam> GetTeams()
		{
			return _dbContext.Teams.ToList();
        }

		[Route("Team")]
		[AllowAnonymous]
		public CSGOTeam GetTeam(Guid teamId)
		{
			return _dbContext.Teams.Find(teamId);
		}

        [Route("Strats")]
        public IHttpActionResult GetTeamStrats(Guid teamId)
        {
            if (!this.UserIsTeamMember(teamId))
            {
                return BadRequest("You're not a member of this team.");
            }
            return Ok(_dbContext.Strategies.Where(t => t.TeamId == teamId).ToList());
        }

		[Route("MapPool")]
		public IHttpActionResult GetTeamMapPool(Guid teamId)
		{
			if (!this.UserIsTeamMember(teamId))
			{
				return BadRequest("You're not a member of this team.");
			}
			List<TeamMapPool> mapPool = _dbContext.TeamMapPool.Where(t => t.TeamId == teamId).ToList();
			return Ok(mapPool);
		}

		[Route("Team")]
		[HttpPost]
		public IHttpActionResult TeamFromSteamGroup(SteamUserGroup group)
		{
			string userid = SteamServiceProvider.SteamUserId(User.Identity.GetUserId());
			if (!SteamServiceProvider.VerifyUserIsGroupAdmin(userid, group.groupID64))
			{
				return BadRequest("User is not a steam group owner for " + group.groupName);
			}

			ApplicationUser user = _dbContext.Users.Find(userid);

			CSGOTeam team = _dbContext.Teams.Add(new CSGOTeam()
			{
				SteamGroupId = group.groupID64,
				TeamName = group.groupName,
				TeamAvatar = group.avatarFull
			});
			team.InitializeDefaults();

            team.Members.Add(new TeamMember()
            {
                Member = user,
                IsActive = true,
                IsAdmin = true
            });

			try
			{
				_dbContext.SaveChanges();
			}
			catch (Exception e)
			{
				return BadRequest(group.groupName + " Steam group has already been registered.");
			}
			return Ok(team);
		}

		[Route("NewTeam")]
		[HttpPost]
		public IHttpActionResult NewTeam(CSGOTeam team)
		{
			string userid = SteamServiceProvider.SteamUserId(User.Identity.GetUserId());

			ApplicationUser user = _dbContext.Users.Find(userid);

			_dbContext.Teams.Add(team);

			team.Members.Add(new TeamMember()
			{
				Member = user,
				IsActive = true,
				IsAdmin = true
			});
			team.InitializeDefaults();

			try
			{
				_dbContext.SaveChanges();
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
			return Ok(team);
		}

		[Route("Member")]
		[HttpPut]
		public IHttpActionResult UpdateTeamMember(TeamMember member)
		{
			if (!UserIsTeamAdmin(member.TeamId))
			{
				return BadRequest("User is not team admin...");
			}
			TeamMember entity = _dbContext.TeamMembers.Find(member.TeamId, member.UserId);
			_dbContext.Entry(entity).CurrentValues.SetValues(member);
			try
			{
				_dbContext.SaveChanges();
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
			return Ok("ok");
		}

		[Route("RemoveMember")]
		[HttpDelete]
		public IHttpActionResult RemoveTeamMember(Guid teamId, string userId)
		{
			if (!UserIsTeamAdmin(teamId))
			{
				return BadRequest("User is not team admin...");
			}
			TeamMember entity = _dbContext.TeamMembers.Find(teamId, userId);
			_dbContext.TeamMembers.Remove(entity);
			try
			{
				_dbContext.SaveChanges();
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
			return Ok("ok");
		}

		[Route("Abandon")]
		[HttpDelete]
		public IHttpActionResult AbandonTeam(Guid teamId)
		{
			string userId = SteamServiceProvider.SteamUserId(User.Identity.GetUserId());
			CSGOTeam team = _dbContext.Teams.Find(teamId);
			TeamMember entity = team.Members.SingleOrDefault(e => e.UserId == userId);
			team.Members.Remove(entity);
			if (team.Members.Count == 0)
			{
				_dbContext.Teams.Remove(team);
			}
			try
			{
				_dbContext.SaveChanges();
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
			return Ok("ok");
		}

		[Route("Invite")]
		[HttpPost]
		public IHttpActionResult InviteToTeam(string userId, CSGOTeam team)
		{
			if (!UserIsTeamAdmin(team.TeamId))
			{
				return BadRequest("User is not team admin for " + team.TeamName);
			}

			CSGOTeam teamEntity = _dbContext.Teams.Find(team.TeamId);
			ApplicationUser invitedUserEntity = _dbContext.Users.Find(userId);
			ApplicationUser invitingUserEntity = _dbContext.Users.Find(SteamServiceProvider.SteamUserId(User.Identity.GetUserId()));
			teamEntity.Invites.Add(new TeamInvite()
			{
				InvitedUser = invitedUserEntity,
				InvitingUser = invitingUserEntity
			});
			try
			{
				_dbContext.SaveChanges();
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
			return Ok(userId);
		}

		[Route("Apply")]
		[HttpPost]
		public IHttpActionResult ApplyToTeam(TeamApplication application)
		{
			if (ModelState.IsValid)
			{
				_dbContext.TeamApplications.Add(application);
				try
				{
					_dbContext.SaveChanges();
				}
				catch (Exception e)
				{
					return BadRequest("Something went wrong...");
				}
				return Ok(application);
			}
			return BadRequest("Something went wrong with your application validation");
		}

		[Route("Applications")]
		public IHttpActionResult GetTeamApplications(Guid teamId)
		{
			if (!UserIsTeamAdmin(teamId))
			{
				return BadRequest("You need to be team admin.");
			}

			return Ok(_dbContext.TeamApplications.Where(i => i.TeamId == teamId).ToList());
		}

        [Route("ApproveApplication")]
        [HttpPut]
		public IHttpActionResult ApproveApplication(TeamApplication application)
		{
			if (!UserIsTeamAdmin(application.TeamId))
			{
				return BadRequest("You need to be team admin.");
			}

			TeamApplication entity = _dbContext.TeamApplications.Find(application.ApplicantId, application.TeamId);
			entity.State = NotificationState.Accepted;

            ApplicationUser user = _dbContext.Users.Find(application.ApplicantId);
            entity.Team.Members.Add(new TeamMember()
            {
                Member = user,
                IsActive = true,
                IsAdmin = false
            });
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

        [Route("RejectApplication")]
        [HttpPut]
        public IHttpActionResult RejectApplication(TeamApplication application)
        {
            if (!UserIsTeamAdmin(application.TeamId))
            {
                return BadRequest("You need to be team admin.");
            }

            TeamApplication entity = _dbContext.TeamApplications.Find(application.ApplicantId, application.TeamId);
            entity.State = NotificationState.Rejected;
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest("Something went wrong...");
            }
            return Ok("ok");
        }

		[Route("MapPool")]
		[HttpPut]
		public IHttpActionResult SetTeamMapPool(List<TeamMapPool> maps)
		{
			if (!this.UserIsTeamAdmin(maps[0].TeamId))
			{
				return BadRequest("You need to be team admin.");
			}

			foreach (TeamMapPool mapPool in maps)
			{
				TeamMapPool entity = _dbContext.TeamMapPool.Find(mapPool.TeamId, mapPool.Map);
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
			if (!this.UserIsTeamAdmin(day.TeamId))
			{
				return BadRequest("You need to be team admin.");
			}

			TeamAvailability entity = _dbContext.TeamPracticeSchedule.Find(day.TeamId, day.Day);
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
			if (!UserIsTeamMember(strategy.TeamId))
			{
				return BadRequest("You need to be team member.");
			}

			TeamStrategy entity = _dbContext.Strategies.Find(strategy.Id);
			if (entity == null)
			{
				_dbContext.Strategies.Add(strategy);
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

		private bool UserIsTeamAdmin(Guid teamId)
		{
			CSGOTeam team = _dbContext.Teams.Find(teamId);
			return team != null && team.Members.Any(m => m.IsAdmin && m.UserId == SteamServiceProvider.SteamUserId(User.Identity.GetUserId()));
		}

        private bool UserIsTeamMember(Guid teamId)
        {
            CSGOTeam team = _dbContext.Teams.Find(teamId);
            return team != null && team.Members.Any(m => m.UserId == SteamServiceProvider.SteamUserId(User.Identity.GetUserId()));
        }
	}
}
