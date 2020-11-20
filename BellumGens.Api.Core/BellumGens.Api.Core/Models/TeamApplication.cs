using Newtonsoft.Json;
using SteamModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Core.Models
{
	public class TeamApplication : Application
	{
		public string ApplicantId { get; set; }

		public Guid TeamId { get; set; }

        [NotMapped]
        public string UserName
        {
            get
            {
                return User?.UserName;
            }
        }

        [NotMapped]
        public string AvatarIcon
        {
            get
            {
                return User?.AvatarIcon;
            }
        }

        [ForeignKey("ApplicantId")]
		[JsonIgnore]
		public virtual ApplicationUser User { get; set; }

		[ForeignKey("TeamId")]
		[JsonIgnore]
		public virtual CSGOTeam Team { get; set; }
	}
}