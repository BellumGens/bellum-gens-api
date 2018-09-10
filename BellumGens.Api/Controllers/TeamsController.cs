using BellumGens.Api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using SteamModels;
using Microsoft.Owin.Security.Cookies;
using System;

namespace BellumGens.Api.Controllers
{
	[EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
	[Authorize]
	[RoutePrefix("api/Teams")]
	public class TeamsController : ApiController
    {
		private BellumGensDbContext _dbContext = new BellumGensDbContext();

		[Route("Teams")]
		[AllowAnonymous]
		public List<CSGOTeam> GetTeams()
		{
			List<CSGOTeam> teams = _dbContext.Teams.ToList();
			return teams;
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
