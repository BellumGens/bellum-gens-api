using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Models
{
	public class Tournament
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public Guid ID { get; set; }

		public string Name { get; set; }

		public DateTimeOffset StartDate { get; set; }

		public DateTimeOffset EndDate { get; set; }

		public virtual ICollection<CSGOTeam> Teams { get; set; }

		public virtual ICollection<TournamentApplication> Applications { get; set; }
	}
}