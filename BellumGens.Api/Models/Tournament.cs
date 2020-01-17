using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Models
{
	public class Tournament
	{
		public Tournament()
		{
			CSGOGroups = new HashSet<TournamentCSGOGroup>();
			SC2Groups = new HashSet<TournamentSC2Group>();
			Applications = new HashSet<TournamentApplication>();
		}

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid ID { get; set; }

		public string Name { get; set; }

		public DateTimeOffset StartDate { get; set; }

		public DateTimeOffset EndDate { get; set; }

		[JsonIgnore]
		public virtual ICollection<TournamentCSGOGroup> CSGOGroups { get; set; }

		[JsonIgnore]
		public virtual ICollection<TournamentSC2Group> SC2Groups { get; set; }

		[JsonIgnore]
		public virtual ICollection<TournamentApplication> Applications { get; set; }
	}
}