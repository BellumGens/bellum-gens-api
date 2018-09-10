using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Models
{
	public class CSGOTeam
	{
        public CSGOTeam() : base()
        {
            this.Members = new HashSet<TeamMember>();
        }

        [Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid TeamId { get; set; }

		public string SteamGroupId { get; set; }

		public string TeamName { get; set; }
		public string TeamAvatar { get; set; }

		public virtual ICollection<TeamMember> Members { get; set; }
	}
}