using BellumGens.Api.Providers;
using Newtonsoft.Json;
using SteamModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Models
{
	public class CSGOTeam
	{
		private SteamGroup _steamGroup;

        public CSGOTeam() : base()
        {
            this.Members = new HashSet<TeamMember>();
			this.Invites = new HashSet<TeamInvite>();
			this.PracticeSchedule = new HashSet<TeamAvailability>();
		}

		public void InitializeDefaults()
		{
			this.PracticeSchedule = new HashSet<TeamAvailability>() {
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
			this.MapPool = new HashSet<TeamMapPool>()
			{
				new TeamMapPool
				{
					Map = CSGOMaps.Cache,
					IsPlayed = true
				},
				new TeamMapPool
				{
					Map = CSGOMaps.Dust2,
					IsPlayed = true
				},
				new TeamMapPool
				{
					Map = CSGOMaps.Inferno,
					IsPlayed = true
				},
				new TeamMapPool
				{
					Map = CSGOMaps.Mirage,
					IsPlayed = true
				},
				new TeamMapPool
				{
					Map = CSGOMaps.Nuke,
					IsPlayed = true
				},
				new TeamMapPool
				{
					Map = CSGOMaps.Overpass,
					IsPlayed = true
				},
				new TeamMapPool
				{
					Map = CSGOMaps.Train,
					IsPlayed = true
				},
				new TeamMapPool
				{
					Map = CSGOMaps.Vertigo,
					IsPlayed = true
				},
				new TeamMapPool
				{
					Map = CSGOMaps.Cobblestone,
					IsPlayed = true
				}
			};

			Visible = true;
		}

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid TeamId { get; set; }

        [MaxLength(64)]
        [Index(IsUnique = true)]
        public string SteamGroupId { get; set; }

		public string TeamName { get; set; }

		public string TeamAvatar { get; set; }

		public string Description { get; set; }

		public string Discord { get; set; }

		public bool Visible { get; set; }

		[JsonIgnore]
		public virtual ICollection<TeamStrategy> Strategies { get; set; }

		public virtual ICollection<TeamAvailability> PracticeSchedule { get; set; }

		public virtual ICollection<TeamMember> Members { get; set; }

		[JsonIgnore]
		public virtual ICollection<TeamInvite> Invites { get; set; }

		[JsonIgnore]
		public virtual ICollection<TeamApplication> Applications { get; set; }

		[JsonIgnore]
		public virtual ICollection<TeamMapPool> MapPool { get; set; }

		[NotMapped]
		public virtual SteamGroup SteamGroup
		{
			get
			{
				if (SteamGroupId != null && _steamGroup == null)
				{
					_steamGroup = SteamServiceProvider.GetSteamGroup(SteamGroupId);
				}
				return _steamGroup;
			}
		}
	}
}