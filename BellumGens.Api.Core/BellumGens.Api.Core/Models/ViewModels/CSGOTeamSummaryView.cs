using System;

namespace BellumGens.Api.Core.Models
{
	public class CSGOTeamSummaryViewModel
	{
		public CSGOTeamSummaryViewModel(CSGOTeam team)
		{
			TeamName = team.TeamName;
			TeamId = team.TeamId;
			TeamAvatar = team.TeamAvatar;
			Description = team.Description;
			CustomUrl = team.CustomUrl;
		}
		public string TeamName { get; set; }
		public Guid TeamId { get; set; }
		public string TeamAvatar { get; set; }
		public string Description { get; set; }
		public string CustomUrl { get; }
	}
}