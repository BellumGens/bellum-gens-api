using BellumGens.Api.Models;
using BellumGens.Api.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace BellumGens.Api.Controllers
{
	[RoutePrefix("api/Strategy")]
	[Authorize]
	public class StrategyController : BaseController
    {
		private readonly BellumGensDbContext _dbContext = new BellumGensDbContext();

		[Route("Strategies")]
		[AllowAnonymous]
		public IHttpActionResult GetStrategies(int page = 0)
		{
			List<CSGOStrategy> strategies = _dbContext.Strategies.Where(s => s.Visible == true && (!string.IsNullOrEmpty(s.Url) || !string.IsNullOrEmpty(s.StratImage)))
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

		[Route("userstrats")]
		public IHttpActionResult GetUserStrats(string userId)
		{
			if (GetAuthUser().Id == userId)
			{
				var strategies = _dbContext.Strategies.Where(s => s.UserId == userId).OrderByDescending(s => s.LastUpdated).ToList();
				return Ok(strategies);
			}
			return BadRequest("You need to authenticate first.");
		}

		[Route("Strat")]
		[AllowAnonymous]
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
                ApplicationUser user = GetAuthUser();
                strategy.UserId = user.Id;
				strategy.UniqueCustomUrl(_dbContext);
				strategy.SaveStrategyImage();
				entity = _dbContext.Strategies.Add(strategy);
			}
			else
			{
                if (string.IsNullOrEmpty(entity.UserId))
                {
                    ApplicationUser user = GetAuthUser();
                    strategy.UserId = user.Id;
                }
				strategy.LastUpdated = DateTimeOffset.Now;
				strategy.SaveStrategyImage();
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

		[Route("Strat")]
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
		public IHttpActionResult SubmitStrategyComment(StrategyComment comment)
		{
			string userId = GetAuthUser().Id;
			CSGOStrategy strat = null;

            comment.User = _dbContext.Users.Find(userId);

			var entity = _dbContext.StrategyComments.Find(comment.Id);
			if (entity != null)
			{
				_dbContext.Entry(entity).CurrentValues.SetValues(comment);
			}
			else
			{
				comment = _dbContext.StrategyComments.Add(comment);
				strat = _dbContext.Strategies.Find(comment.StratId);
			}

			try
			{
				_dbContext.SaveChanges();
			}
			catch
			{
				return BadRequest("Something went wrong...");
			}

			if (strat != null && strat.UserId != userId)
			{
				List<BellumGensPushSubscription> subs = _dbContext.PushSubscriptions.Where(s => s.userId == comment.Strategy.UserId).ToList();
				NotificationsService.SendNotification(subs, comment);
			}
			return Ok(comment);
		}

		[Route("Comment")]
		[HttpDelete]
		public IHttpActionResult DeleteStrategyComment(Guid id)
		{
			string userId = GetAuthUser().Id;

			var comment = _dbContext.StrategyComments.Find(id);
			if (comment == null || comment.UserId != userId)
			{
				return BadRequest("Could not delete this user comment...");
			}

			_dbContext.StrategyComments.Remove(comment);
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
            CSGOStrategy strat = _dbContext.Strategies.Find(id);
            if (strat?.TeamId != null)
            {
                if (strat.Team.Members.Any(m => m.IsEditor || m.IsAdmin && m.UserId == user.Id))
                {
                    return strat;
                }
            }
            else if (strat?.UserId == user.Id)
            {
                return strat;
            }
            return null;
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
