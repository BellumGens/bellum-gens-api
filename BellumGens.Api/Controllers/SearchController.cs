using BellumGens.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

		[Route("Search")]
		[HttpGet]
		public IHttpActionResult Search(string name)
		{
			SearchResultViewModel results = new SearchResultViewModel();
			if (!string.IsNullOrEmpty(name))
			{
				results.Teams = _dbContext.Teams.Where(t => t.TeamName.Contains(name)).ToList();
				List<string> activeUsers = _dbContext.Users.Where(u => u.UserName.Contains(name)).Select(e => e.Id).ToList();
				foreach (string user in activeUsers)
				{
					results.Players.Add(SteamServiceProvider.GetSteamUserDetails(user));
				}
				return Ok(results);
			}
			return Ok(results);
		}

		[Route("Teams")]
		[HttpGet]
		public IHttpActionResult SearchTeams(PlaystyleRole? role, double overlap)
		{
			if (overlap <= 0 && role == null)
			{
				return Ok(_dbContext.Teams.OrderBy(t => t.TeamId).Take(50).ToList());
			}

			List<CSGOTeam> teams;
			if (role != null)
			{
				teams = _dbContext.Teams.Where(t => !t.Members.Any(m => m.Role == role) && t.PracticeSchedule.Any(d => d.Available)).ToList();
			}
			else
			{
				teams = _dbContext.Teams.Where(t => t.PracticeSchedule.Any(d => d.Available)).ToList();
			}
			if (overlap > 0)
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
				overlap = Math.Min(overlap, user.GetTotalAvailability());
				return Ok(teams.Where(t => t.GetTotalAvailability() >= overlap && t.GetTotalOverlap(user) >= overlap));
			}
			return Ok(teams);
		}

		[Route("Players")]
		[HttpGet]
		public IHttpActionResult SearchPlayers(PlaystyleRole? role, double overlap, Guid? teamid)
		{
			List<UserStatsViewModel> steamUsers = new List<UserStatsViewModel>();

			if (overlap <= 0 && role == null)
			{
				var ids = _dbContext.Users.OrderBy(u => u.Id).Select(u => u.Id).Take(50).ToList();
				foreach (string user in ids)
				{
					steamUsers.Add(SteamServiceProvider.GetSteamUserDetails(user));
				}
				return Ok(steamUsers);
			}

			List<ApplicationUser> users;

			if (role != null)
			{
				users = _dbContext.Users.Where(u => u.PreferredPrimaryRole == role || u.PreferredSecondaryRole == role).ToList();
			}
			else
			{
				users = _dbContext.Users.Where(u => u.Availability.Any(d => d.Available)).ToList();
			}
			if (overlap > 0)
			{
				if (!User.Identity.IsAuthenticated)
				{
					return BadRequest("You must sign in to perform search by availability...");
				}
				
				List<string> userIds;
				if (teamid != null)
				{
					CSGOTeam team = _dbContext.Teams.Find(teamid);
					overlap = Math.Min(overlap, team.GetTotalAvailability());
					userIds = users.Where(u => u.GetTotalAvailability() >= overlap && team.GetTotalOverlap(u) >= overlap).Select(u => u.Id).ToList();
				}
				else
				{
					ApplicationUser user = _dbContext.Users.Find(SteamServiceProvider.SteamUserId(User.Identity.GetUserId()));
					overlap = Math.Min(overlap, user.GetTotalAvailability());
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
