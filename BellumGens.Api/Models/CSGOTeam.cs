using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Models
{
	public class CSGOTeam
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string TeamId { get; set; }

		public string SteamGroupId { get; set; }

		public string TeamName { get; set; }
		public string TeamAvatar { get; set; }

		public virtual ICollection<TeamMembers> Members { get; set; }
	}
}