﻿using System;

namespace BellumGens.Api.Models
{
	public class CSGOTeamSummaryViewModel
	{
		public CSGOTeamSummaryViewModel(CSGOTeam team)
		{
			TeamName = team.TeamName;
			TeamId = team.TeamId;
			TeamAvatar = team.TeamAvatar;
		}
		public string TeamName { get; set; }
		public Guid TeamId { get; set; }
		public string TeamAvatar { get; set; }
	}
}