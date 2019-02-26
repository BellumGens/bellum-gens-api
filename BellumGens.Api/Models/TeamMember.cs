﻿using BellumGens.Api.Providers;
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
        public SteamUser SteamUser
        {
            get
            {
				if (Member != null)
				{
					return Member.SteamUser;
				}
				return null;
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