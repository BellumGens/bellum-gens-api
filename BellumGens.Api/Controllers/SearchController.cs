using BellumGens.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using BellumGens.Api.Models.Extensions;
using BellumGens.Api.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;

namespace BellumGens.Api.Controllers
{
	[EnableCors(origins: CORSConfig.allowedOrigins, headers: CORSConfig.allowedHeaders, methods: CORSConfig.allowedMethods, SupportsCredentials = true)]
	[HostAuthentication(CookieAuthenticationDefaults.AuthenticationType)]
	[RoutePrefix("api/Search")]
	public class SearchController : ApiController
	{
		private BellumGensDbContext _dbContext = new BellumGensDbContext();

		[Route("Teams")]
		[HttpPost]
		public IHttpActionResult SearchTeams(TeamSearchModel model)
		{
			if (model.scheduleOverlap <= 0 && model.role == null && string.IsNullOrEmpty(model.name))
			{
				return BadRequest("No search criteria provided...");
			}
			if (!string.IsNullOrEmpty(model.name))
			{
				return Ok(_dbContext.Teams.Where(t => t.TeamName.Contains(model.name)).ToList());
			}
			List<CSGOTeam> teams;
			if (model.role != null)
			{
				teams = _dbContext.Teams.Where(t => !t.Members.Any(m => m.Role == (PlaystyleRole)model.role) && t.PracticeSchedule.Any(d => d.Available)).ToList();
			}
			else
			{
				teams = _dbContext.Teams.Where(t => t.PracticeSchedule.Any(d => d.Available)).ToList();
			}
			if (model.scheduleOverlap > 0)
			{
				if (!User.Identity.IsAuthenticated)
				{
					return BadRequest("You must sign in to perform search by availability...");
				}
				ApplicationUser user = _dbContext.Users.Find(SteamServiceProvider.SteamUserId(User.Identity.GetUserId()));
				if (!user.Availability.Any(a => a.Available))
				{
					return BadRequest("You must provide your availability in your user profile...");
				}
				double overlap = Math.Min(model.scheduleOverlap, user.GetTotalAvailability());
				return Ok(teams.Where(t => t.GetTotalAvailability() >= overlap && t.GetTotalOverlap(user) >= overlap));
			}
			return Ok(teams);
		}

		[Route("Players")]
		[HttpPost]
		public IHttpActionResult SearchPlayers(PlayerSearchModel model)
		{
			if (model.scheduleOverlap <= 0 && model.role == null && string.IsNullOrEmpty(model.name))
			{
				return BadRequest("No search criteria provided...");
			}

			List<UserStatsViewModel> steamUsers = new List<UserStatsViewModel>();
			if (!string.IsNullOrEmpty(model.name))
			{
				List<string> activeUsers = _dbContext.Users.Where(u => u.UserName.Contains(model.name)).Select(e => e.Id).ToList();
				foreach (string user in activeUsers)
				{
					steamUsers.Add(SteamServiceProvider.GetSteamUserDetails(user));
				}
				return Ok(steamUsers);
			}
			List<ApplicationUser> users;

			if (model.role != null)
			{
				users = _dbContext.Users.Where(u => u.PreferredPrimaryRole == model.role || u.PreferredSecondaryRole == model.role).ToList();
			}
			else
			{
				users = _dbContext.Users.Where(u => u.Availability.Any(d => d.Available)).ToList();
			}
			if (model.scheduleOverlap > 0)
			{
				if (!User.Identity.IsAuthenticated)
				{
					return BadRequest("You must sign in to perform search by availability...");
				}

				double overlap = 0;
				List<string> userIds;
				if (model.teamId != null)
				{
					CSGOTeam team = _dbContext.Teams.Find(model.teamId);
					overlap = Math.Min(model.scheduleOverlap, team.GetTotalAvailability());
					userIds = users.Where(u => u.GetTotalAvailability() >= overlap && team.GetTotalOverlap(u) >= overlap).Select(u => u.Id).ToList();
				}
				else
				{
					ApplicationUser user = _dbContext.Users.Find(SteamServiceProvider.SteamUserId(User.Identity.GetUserId()));
					overlap = Math.Min(model.scheduleOverlap, user.GetTotalAvailability());
					if (!user.Availability.Any(a => a.Available))
					{
						return BadRequest("You must provide your availability in your user profile...");
					}
					userIds = users.Where(u => u.GetTotalAvailability() >= overlap && u.GetTotalOverlap(user) >= overlap).Select(u => u.Id).ToList();
				}
				foreach (string user in userIds)
				{
					steamUsers.Add(SteamServiceProvider.GetSteamUserDetails(user));
				}
				return Ok(steamUsers);
			}
			
			foreach (string user in users.Select(u => u.Id))
			{
				steamUsers.Add(SteamServiceProvider.GetSteamUserDetails(user));
			}
			return Ok(steamUsers);
		}
	}
}
