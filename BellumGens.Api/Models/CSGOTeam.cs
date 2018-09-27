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
			this.Invites = new HashSet<TeamInvites>();
        }

        [Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid TeamId { get; set; }

        [MaxLength(64)]
        [Index(IsUnique = true)]
        public string SteamGroupId { get; set; }

		public string TeamName { get; set; }

		public string TeamAvatar { get; set; }

		public virtual ICollection<TeamMember> Members { get; set; }

		public virtual ICollection<TeamInvites> Invites { get; set; }
	}
}