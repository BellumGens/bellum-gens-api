using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Core.Models
{
	public class TeamInvite : Application
	{
		private CSGOTeamSummaryViewModel _teamInfo;

		public string InvitingUserId { get; set; }
		public string InvitedUserId { get; set; }
		public Guid TeamId { get; set; }

		[NotMapped]
		public CSGOTeamSummaryViewModel TeamInfo
		{
			get
			{
				if (_teamInfo == null && Team != null)
				{
					_teamInfo = new CSGOTeamSummaryViewModel(Team);
				}
				return _teamInfo;
			}
		}

		[ForeignKey("TeamId")]
		[JsonIgnore]
		public virtual CSGOTeam Team { get; set; }

		[ForeignKey("InvitingUserId")]
		[JsonIgnore]
		public virtual ApplicationUser InvitingUser { get; set; }

		[ForeignKey("InvitedUserId")]
		[JsonIgnore]
		public virtual ApplicationUser InvitedUser { get; set; }
	}
}