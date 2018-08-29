using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BellumGens.Api.Models
{
	public class CSGOTeam
	{
		[Key]
		public string TeamId { get; set; }

		public string TeamName { get; set; }
		public string TeamAvatar { get; set; }

		public virtual ICollection<ApplicationUser> Members { get; set; }
	}
}