using System.Collections.Generic;

namespace BellumGens.Api.Models
{
	public class SearchResultViewModel
	{
		public List<CSGOTeam> Teams { get; set; } = new List<CSGOTeam>();
		public List<CSGOStrategy> Strategies { get; set; } = new List<CSGOStrategy>();
		public UserStatsViewModel[] Players { get; set; }
	}
}