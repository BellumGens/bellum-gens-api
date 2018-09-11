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
	[EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*", SupportsCredentials = true)]
	[Authorize]
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
		public CSGOTeam GetTeam(string teamId)
		{
			return _dbContext.Teams.Find(teamId);
		}

		[Route("Team")]
		[HttpPost]
		[HostAuthentication(CookieAuthenticationDefaults.AuthenticationType)]
		public IHttpActionResult TeamFromSteamGroup(SteamUserGroup group)
		{
			CSGOTeam team = _dbContext.Teams.Add(new CSGOTeam()
			{
				SteamGroupId = group.groupID64,
				TeamName = group.groupName,
				TeamAvatar = group.avatarMedium
			});

            ApplicationUser user = _dbContext.Users.Find(SteamServiceProvider.SteamUserId(User.Identity.GetUserId()));

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
	}
}
