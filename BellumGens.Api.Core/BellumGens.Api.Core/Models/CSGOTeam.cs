using BellumGens.Api.Core.Providers;
using BellumGens.Api.Common;
using Newtonsoft.Json;
using SteamModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BellumGens.Api.Core.Models
{
	public class CSGOTeam
	{
		public void InitializeDefaults()
		{
			PracticeSchedule = new HashSet<TeamAvailability>() {
				new TeamAvailability
				{
					Day = DayOfWeek.Monday
				},
				new TeamAvailability
				{
					Day = DayOfWeek.Tuesday
				},
				new TeamAvailability
				{
					Day = DayOfWeek.Wednesday
				},
				new TeamAvailability
				{
					Day = DayOfWeek.Thursday
				},
				new TeamAvailability
				{
					Day = DayOfWeek.Friday
				},
				new TeamAvailability
				{
					Day = DayOfWeek.Saturday
				},
				new TeamAvailability
				{
					Day = DayOfWeek.Sunday
				}
			};
			MapPool = new HashSet<TeamMapPool>()
			{
				new TeamMapPool
				{
					Map = CSGOMap.Cache,
					IsPlayed = true
				},
				new TeamMapPool
				{
					Map = CSGOMap.Dust2,
					IsPlayed = true
				},
				new TeamMapPool
				{
					Map = CSGOMap.Inferno,
					IsPlayed = true
				},
				new TeamMapPool
				{
					Map = CSGOMap.Mirage,
					IsPlayed = true
				},
				new TeamMapPool
				{
					Map = CSGOMap.Nuke,
					IsPlayed = true
				},
				new TeamMapPool
				{
					Map = CSGOMap.Overpass,
					IsPlayed = true
				},
				new TeamMapPool
				{
					Map = CSGOMap.Train,
					IsPlayed = true
				},
				new TeamMapPool
				{
					Map = CSGOMap.Vertigo,
					IsPlayed = true
				},
				new TeamMapPool
				{
					Map = CSGOMap.Cobblestone,
					IsPlayed = true
				}
			};
		}

		public void UniqueCustomUrl(BellumGensDbContext context)
		{
			if (string.IsNullOrEmpty(CustomUrl))
			{
				var parts = TeamName.Split(' ');
				string url = string.Join("-", parts);
				while (context.Teams.Where(t => t.CustomUrl == url).SingleOrDefault() != null)
				{
					if (url.Length > 58)
						url = url.Substring(0, 58);
					url += '-' + Util.GenerateHashString(6);
				}
				CustomUrl = url;
			}
		}

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid TeamId { get; set; }

        [MaxLength(64)]
        public string SteamGroupId { get; set; }

		public string TeamName { get; set; }

		public string TeamAvatar { get; set; }

		public string Description { get; set; }

		public string Discord { get; set; }

		public DateTimeOffset RegisteredOn { get; set; } = DateTimeOffset.Now;

		public bool Visible { get; set; } = true;

		[MaxLength(64)]
		public string CustomUrl { get; set; }

		[JsonIgnore]
		public virtual ICollection<CSGOStrategy> Strategies { get; set; } = new HashSet<CSGOStrategy>();

		public virtual ICollection<TeamAvailability> PracticeSchedule { get; set; }

		public virtual ICollection<TeamMember> Members { get; set; } = new HashSet<TeamMember>();

		[JsonIgnore]
		public virtual ICollection<TeamInvite> Invites { get; set; } = new HashSet<TeamInvite>();

		[JsonIgnore]
		public virtual ICollection<TeamApplication> Applications { get; set; } = new HashSet<TeamApplication>();

		[JsonIgnore]
		public virtual ICollection<TeamMapPool> MapPool { get; set; }

		//[NotMapped]
		//public virtual SteamGroup SteamGroup
		//{
		//	get
		//	{
		//		if (SteamGroupId != null && _steamGroup == null)
		//		{
		//			_steamGroup = _steamService.GetSteamGroup(SteamGroupId).Result;
		//		}
		//		return _steamGroup;
		//	}
		//}
	}
}