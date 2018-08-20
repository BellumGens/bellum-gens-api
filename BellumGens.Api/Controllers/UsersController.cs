using BellumGens.Api.Models;
using BellumGens.Api.Providers;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using System.Web.Http;

namespace BellumGens.Api.Controllers
{
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
			user.availability = _dbContext.Users.Find(user.steamUser.steamID64).Availability;
			return user;
		}
	}
}
