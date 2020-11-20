using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Core.Models
{
	public class TeamMember
	{
		public Guid TeamId { get; set; }

		public string UserId { get; set; }

		public bool IsActive { get; set; }

		public bool IsAdmin { get; set; }

		public bool IsEditor { get; set; }

		public PlaystyleRole Role { get; set; }

        [NotMapped]
        public string Username
        { 
            get
            {
                return Member?.UserName;
            }
        }

        [NotMapped]
        public string AvatarIcon
        {
            get
            {
                return Member?.AvatarIcon;
            }
        }

        [NotMapped]
        public string AvatarMedium
        {
            get
            {
                return Member?.AvatarMedium;
            }
        }

        [NotMapped]
        public string CustomUrl
        {
            get
            {
                return Member?.CustomUrl;
            }
        }

        [NotMapped]
        public string AvatarFull
        {
            get
            {
                return Member?.AvatarFull;
            }
        }

        [NotMapped]
        public string Country
        {
            get
            {
                return Member?.Country;
            }
        }

        [NotMapped]
        public string RealName
        {
            get
            {
                return Member?.RealName;
            }
        }

        [ForeignKey("TeamId")]
        [JsonIgnore]
		public virtual CSGOTeam Team { get; set; } 

		[ForeignKey("UserId")]
        [JsonIgnore]
		public virtual ApplicationUser Member { get; set; }
	}
}