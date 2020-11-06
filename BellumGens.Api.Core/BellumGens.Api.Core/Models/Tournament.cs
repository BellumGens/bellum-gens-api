using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Core.Models
{
	public class Tournament
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid ID { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public string Logo { get; set; }

		public DateTimeOffset StartDate { get; set; }

		public DateTimeOffset EndDate { get; set; }

		public bool Active { get; set; } = false;

		[JsonIgnore]
		public virtual ICollection<TournamentCSGOMatch> CSGOMatches { get; set; } = new HashSet<TournamentCSGOMatch>();

		[JsonIgnore]
		public virtual ICollection<TournamentSC2Match> SC2Matches { get; set; } = new HashSet<TournamentSC2Match>();

		[JsonIgnore]
		public virtual ICollection<TournamentCSGOGroup> CSGOGroups { get; set; } = new HashSet<TournamentCSGOGroup>();

		[JsonIgnore]
		public virtual ICollection<TournamentSC2Group> SC2Groups { get; set; } = new HashSet<TournamentSC2Group>();

		[JsonIgnore]
		public virtual ICollection<TournamentApplication> Applications { get; set; } = new HashSet<TournamentApplication>();
	}
}