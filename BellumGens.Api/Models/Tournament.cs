using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Models
{
	public class Tournament
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid ID { get; set; }

		public string Name { get; set; }

		public DateTimeOffset StartDate { get; set; }

		public DateTimeOffset EndDate { get; set; }

		public ICollection<TournamentCSGOMatch> CSGOMatches { get; set; }

		public ICollection<TournamentSC2Match> SC2Matches { get; set; }
		//public virtual ICollection<CSGOTeam> Teams { get; set; }

		//public virtual ICollection<TournamentApplication> Applications { get; set; }
	}
}