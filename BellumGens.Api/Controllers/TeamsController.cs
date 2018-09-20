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
		public IHttpActionResult TeamFromSteamGroup(CSGOTeam team)
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
			return Ok(member);
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
			return Ok();
		}

		private bool UserIsTeamAdmin(Guid teamId)
		{
			CSGOTeam team = _dbContext.Teams.Find(teamId);
			return team != null && team.Members.Any(m => m.IsAdmin && m.UserId == SteamServiceProvider.SteamUserId(User.Identity.GetUserId()));
		}
	}
}
