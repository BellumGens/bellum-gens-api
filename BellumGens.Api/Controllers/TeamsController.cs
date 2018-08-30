using BellumGens.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BellumGens.Api.Controllers
{
	[EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
	[RoutePrefix("api/Teams")]
	public class TeamsController : ApiController
    {
		private BellumGensDbContext _dbContext = new BellumGensDbContext();

		[Route("Teams")]
		public List<CSGOTeam> GetTeams()
		{
			List<CSGOTeam> teams = _dbContext.Teams.ToList();
			return teams;
		}
	}
}
