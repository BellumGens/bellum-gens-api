
using System.Collections.Generic;

namespace BellumGens.Api.Models
{
	public class SearchResultViewModel
	{
		public SearchResultViewModel()
		{
			this.Teams = new List<CSGOTeam>();
			this.Players = new List<UserStatsViewModel>();
		}

		public List<CSGOTeam> Teams { get; set; }
		public List<UserStatsViewModel> Players { get; set; }
	}
}