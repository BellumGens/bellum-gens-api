using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BellumGens.Api.Models
{
	public class TeamMembers
	{
		public string TeamId { get; set; }

		public string UserId { get; set; }

		public bool IsActive { get; set; }

		public bool IsAdmin { get; set; }

		public PlaystyleRole Role { get; set; }

		[ForeignKey("TeamId")]
		public virtual CSGOTeam Team { get; set; } 

		[ForeignKey("UserId")]
		public virtual ApplicationUser Member { get; set; }
	}
}