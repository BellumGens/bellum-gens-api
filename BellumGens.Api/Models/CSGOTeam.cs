﻿using Newtonsoft.Json;
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
					Map = CSGOMaps.Cache
				},
				new TeamMapPool
				{
					Map = CSGOMaps.Dust2
				},
				new TeamMapPool
				{
					Map = CSGOMaps.Inferno
				},
				new TeamMapPool
				{
					Map = CSGOMaps.Mirage
				},
				new TeamMapPool
				{
					Map = CSGOMaps.Nuke
				},
				new TeamMapPool
				{
					Map = CSGOMaps.Overpass
				},
				new TeamMapPool
				{
					Map = CSGOMaps.Train
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
	}
}