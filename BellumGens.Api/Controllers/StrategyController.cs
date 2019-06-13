using BellumGens.Api.Models;
using Microsoft.Owin.Security.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BellumGens.Api.Controllers
{
	[EnableCors(origins: CORSConfig.allowedOrigins, headers: CORSConfig.allowedHeaders, methods: CORSConfig.allowedMethods, SupportsCredentials = true)]
	[HostAuthentication(CookieAuthenticationDefaults.AuthenticationType)]
	[RoutePrefix("api/Strategy")]
	public class StrategyController : ApiController
    {
		private BellumGensDbContext _dbContext = new BellumGensDbContext();

		public IHttpActionResult GetStrategies(int page = 0)
		{
			List<CSGOStrategy> strategies = _dbContext.Strategies.Where(s => s.TeamId == Guid.Empty && s.Visible == true).OrderByDescending(s => s.Id).Skip(page * 25).Take(25).ToList();
			return Ok(strategies);
		}
	}
}
