using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Models
{
	public class TeamInvite
	{
		private CSGOTeam _teamInfo;

		[Key]
		[Column(Order = 0)]
		public string InvitingUserId { get; set; }

		[Key]
		[Column(Order = 1)]
		public string InvitedUserId { get; set; }

		[Key]
		[Column(Order = 2)]
		public Guid TeamId { get; set; }

		public NotificationState State { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTimeOffset Sent { get; set; }

		[NotMapped]
		public CSGOTeam TeamInfo
		{
			get
			{
				if (_teamInfo == null && Team != null)
				{
					_teamInfo = new CSGOTeam()
					{
						TeamId = Team.TeamId,
						TeamName = Team.TeamName,
						TeamAvatar = Team.TeamAvatar
					};
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

	public enum NotificationState
	{
		NotSeen,
		Seen,
		Rejected,
		Accepted
	}
}