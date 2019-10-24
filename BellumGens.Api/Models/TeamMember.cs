using BellumGens.Api.Providers;
using Newtonsoft.Json;
using SteamModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Models
{
	public class TeamMember
	{
        [Key]
        [Column(Order = 0)]
		public Guid TeamId { get; set; }

        [Key]
        [Column(Order = 1)]
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

        [ForeignKey("TeamId")]
        [JsonIgnore]
		public virtual CSGOTeam Team { get; set; } 

		[ForeignKey("UserId")]
        [JsonIgnore]
		public virtual ApplicationUser Member { get; set; }
	}
}