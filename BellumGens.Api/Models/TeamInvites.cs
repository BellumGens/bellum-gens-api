using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Models
{
	public class TeamInvites
	{
		[Key]
		[Column(Order = 0)]
		public string InvitingUserId { get; set; }

		[Key]
		[Column(Order = 1)]
		public string InvitedUserId { get; set; }

		[Key]
		[Column(Order = 2)]
		public Guid TeamId { get; set; }

		public InviteState State { get; set; }

		public DateTimeOffset Sent { get; set; }

		[ForeignKey("TeamId")]
		public virtual CSGOTeam Team { get; set; }

		[ForeignKey("InvitingUserId")]
		public virtual ApplicationUser InvitingUser { get; set; }

		[ForeignKey("InvitedUserId")]
		public virtual ApplicationUser InvitedUser { get; set; }
	}

	public enum InviteState
	{
		NotSeen,
		Seen,
		Rejected,
		Accepted
	}
}