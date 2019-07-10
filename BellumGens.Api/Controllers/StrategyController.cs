using BellumGens.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace BellumGens.Api.Controllers
{
	[RoutePrefix("api/Strategy")]
	public class StrategyController : BaseController
    {
		private BellumGensDbContext _dbContext = new BellumGensDbContext();

		[Route("Strategies")]
		public IHttpActionResult GetStrategies(int page = 0)
		{
			List<CSGOStrategy> strategies = _dbContext.Strategies.Where(s => s.TeamId == Guid.Empty || s.Visible == true).OrderByDescending(s => s.Id).Skip(page * 25).Take(25).ToList();
			return Ok(strategies);
		}

		[Route("Strategy")]
		[HttpPost]
		public IHttpActionResult SubmitStrategy(CSGOStrategy strategy)
		{
			CSGOTeam team = UserIsTeamEditor(strategy.TeamId);
			if (team == null)
			{
				return BadRequest("You need to be team editor.");
			}

			CSGOStrategy entity = team.Strategies.FirstOrDefault(s => s.Id == strategy.Id);
			if (entity == null)
			{
				entity = _dbContext.Strategies.Add(strategy);
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

		[Route("Strategy")]
		[HttpDelete]
		public IHttpActionResult DeleteStrategy(Guid id, string teamid)
		{
			CSGOTeam team = UserIsTeamEditor(teamid);
			if (team == null)
			{
				return BadRequest("You need to be team editor.");
			}

			CSGOStrategy entity = team.Strategies.FirstOrDefault(s => s.Id == id);
			if (entity != null)
			{
				_dbContext.Strategies.Remove(entity);
			}

			try
			{
				_dbContext.SaveChanges();
			}
			catch
			{
				return BadRequest("Something went wrong...");
			}
			return Ok("Ok");
		}

		private CSGOTeam UserIsTeamEditor(string teamId)
		{
			CSGOTeam team = ResolveTeam(teamId);
			ApplicationUser user = GetAuthUser();
			return team != null && team.Members.Any(m => m.IsEditor || m.IsAdmin && m.UserId == user.Id) ? team : null;
		}

		private CSGOTeam UserIsTeamEditor(Guid teamId)
		{
			CSGOTeam team = _dbContext.Teams.Find(teamId);
			ApplicationUser user = GetAuthUser();
			return team != null && team.Members.Any(m => m.IsEditor || m.IsAdmin && m.UserId == user.Id) ? team : null;
		}

		private CSGOTeam ResolveTeam(string teamId)
		{
			CSGOTeam team = _dbContext.Teams.FirstOrDefault(t => t.CustomUrl == teamId);
			if (team == null)
			{
				var valid = Guid.TryParse(teamId, out Guid id);
				if (valid)
				{
					team = _dbContext.Teams.Find(id);
				}
			}
			return team;
		}
	}
}
