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
			List<CSGOStrategy> strategies = _dbContext.Strategies.Where(s => s.Visible == true && (!string.IsNullOrEmpty(s.Url) || !string.IsNullOrEmpty(s.Image)))
																 .OrderByDescending(s => s.LastUpdated).Skip(page * 25).Take(25).ToList();
			return Ok(strategies.OrderByDescending(s => s.Rating));
		}

		[Route("teamstrats")]
		public IHttpActionResult GetTeamStrats(string teamId)
		{
			CSGOTeam team = UserIsTeamMember(teamId);
			if (team == null)
			{
				return BadRequest("You're not a member of this team.");
			}
			return Ok(team.Strategies);
		}

		[Route("Strat")]
		public IHttpActionResult GetStrat(string stratId)
		{
			CSGOStrategy strat = ResolveStrategy(stratId);
			if (strat != null && !strat.Visible && strat.TeamId != null && strat.TeamId != Guid.Empty)
			{
				if (!UserIsTeamMember(strat.TeamId.Value))
				{
					return BadRequest("You need to be team editor.");
				}
			}

			if (strat != null)
			{
				return Ok(strat);
			}
			return BadRequest("Strat not found or user is not team member.");
		}

		[Route("Strategy")]
		[HttpPost]
		public IHttpActionResult SubmitStrategy(CSGOStrategy strategy)
		{
			if (strategy.TeamId != null && strategy.TeamId != Guid.Empty)
			{
				if (!UserIsTeamEditor(strategy.TeamId.Value))
				{
					return BadRequest("You need to be team editor.");
				}
			}

			CSGOStrategy entity = UserCanEdit(strategy.Id);
			if (entity == null)
			{
				strategy.UniqueCustomUrl(_dbContext);
				entity = _dbContext.Strategies.Add(strategy);
			}
			else
			{
				strategy.LastUpdated = DateTimeOffset.Now;
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
		public IHttpActionResult DeleteStrategy(Guid id)
		{
			CSGOStrategy entity = UserCanEdit(id);
			if (entity == null)
			{
				return BadRequest("You need to be team editor.");
			}

			if (entity.TeamId != null && entity.TeamId != Guid.Empty)
			{
				if (!UserIsTeamEditor(entity.TeamId.Value))
				{
					return BadRequest("You need to be team editor to delete this strategy.");
				}
			}

			_dbContext.Strategies.Remove(entity);

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

		[Route("Vote")]
		[HttpPost]
		[Authorize]
		public IHttpActionResult SubmitStrategyVote(VoteModel model)
		{
			string userId = GetAuthUser().Id;

			var strategy = _dbContext.Strategies.Find(model.id);
			StrategyVote vote = strategy.Votes.FirstOrDefault(v => v.UserId == userId);
			if (vote == null)
			{
				vote = new StrategyVote()
				{
					UserId = userId,
					Vote = model.direction
				};
				strategy.Votes.Add(vote);
			}
			else
			{
				if (vote.Vote == model.direction)
				{
					strategy.Votes.Remove(vote);
					vote = null;
				}
				else
				{
					vote.Vote = model.direction;
				}
			}

			try
			{
				_dbContext.SaveChanges();
			}
			catch
			{
				return BadRequest("Something went wrong...");
			}
			return Ok(vote);
		}

		[Route("Comment")]
		[HttpPost]
		[Authorize]
		public IHttpActionResult SubmitStrategyComment(StrategyComment comment)
		{
			string userId = GetAuthUser().Id;

            comment.User = _dbContext.Users.Find(userId);
			comment = _dbContext.StrategyComments.Add(comment);

            /* TODO: Send push notification to owner */

			try
			{
				_dbContext.SaveChanges();
			}
			catch
			{
				return BadRequest("Something went wrong...");
			}
			return Ok(comment);
		}

		public class VoteModel
		{
			public Guid id { get; set; }
			public VoteDirection direction { get; set; }
		}


		private CSGOStrategy UserCanEdit(Guid id)
		{
			ApplicationUser user = GetAuthUser();
			return _dbContext.Strategies.FirstOrDefault(s => s.Id == id && s.UserId == user.Id);
		}

		private bool UserIsTeamEditor(Guid teamId)
		{
			CSGOTeam team = _dbContext.Teams.Find(teamId);
			ApplicationUser user = GetAuthUser();
			return team != null && team.Members.Any(m => m.IsEditor || m.IsAdmin && m.UserId == user.Id);
		}

		private CSGOTeam UserIsTeamMember(string teamId)
		{
			CSGOTeam team = ResolveTeam(teamId);
			ApplicationUser user = GetAuthUser();
			return team != null && team.Members.Any(m => m.UserId == user.Id) ? team : null;
		}

		private bool UserIsTeamMember(Guid teamId)
		{
			CSGOTeam team = _dbContext.Teams.Find(teamId);
			ApplicationUser user = GetAuthUser();
			return team != null && team.Members.Any(m => m.UserId == user.Id);
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

		private CSGOStrategy ResolveStrategy(string stratId)
		{
			CSGOStrategy strat = _dbContext.Strategies.FirstOrDefault(s => s.CustomUrl == stratId);
			if (strat == null)
			{
				var valid = Guid.TryParse(stratId, out Guid id);
				if (valid)
				{
					strat = _dbContext.Strategies.Find(id);
				}
			}
			return strat;
		}
	}
}
