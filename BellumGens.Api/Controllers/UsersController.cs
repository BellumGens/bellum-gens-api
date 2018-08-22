using BellumGens.Api.Models;
using BellumGens.Api.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BellumGens.Api.Controllers
{
	[EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
	[RoutePrefix("api/Users")]
	public class UsersController : ApiController
    {
		private BellumGensDbContext _dbContext = new BellumGensDbContext();

		[Route("ActiveUsers")]
		public List<UserStatsViewModel> GetActiveUsers()
		{
			List<UserStatsViewModel> steamUsers = new List<UserStatsViewModel>();
			Cache cache = HttpContext.Current.Cache;
			if (!(cache["activeUsers"] is List<string> activeUsers))
				activeUsers = new List<string>();
			foreach (string user in activeUsers)
			{
				steamUsers.Add(SteamServiceProvider.GetSteamUserDetails(user));
			}

			return steamUsers;
		}

		[Route("User")]
		public UserStatsViewModel GetUser(string userid)
		{
			UserStatsViewModel user = SteamServiceProvider.GetSteamUserDetails(userid);
			var registered = _dbContext.Users.Find(user.steamUser.steamID64);
			if (registered != null)
			{
				user.availability = registered.Availability;
			}
			return user;
		}

		[EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*", SupportsCredentials = true)]
		[HostAuthentication(CookieAuthenticationDefaults.AuthenticationType)]
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
	}
}
