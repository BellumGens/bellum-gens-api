
using System.Collections.Generic;

namespace BellumGens.Api.Models
{
	public class SearchResultViewModel
	{
		public SearchResultViewModel()
		{
			this.Teams = new List<CSGOTeam>();
		}

		public List<CSGOTeam> Teams { get; set; }
		public UserStatsViewModel [] Players { get; set; }
	}
}