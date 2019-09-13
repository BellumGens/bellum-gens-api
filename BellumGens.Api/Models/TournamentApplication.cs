using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BellumGens.Api.Models
{
	public class TournamentApplication
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public Guid Id { get; set; }

		public string TournamentId { get; set; }

		public Guid TeamId { get; set; }

		public Guid CompanyId { get; set; }

		public DateTimeOffset DateSubmitted { get; set; } = DateTimeOffset.Now;

		public Game Game { get; set; }

		[ForeignKey("CompanyId")]
		public virtual Company Company { get; set; }

		[ForeignKey("TeamId")]
		public virtual CSGOTeam Team { get; set; }

		[ForeignKey("TournamentId")]
		public virtual Tournament Tournament { get; set; }
	}

	public enum Game
	{
		CSGO,
		StarCraft2
	}
}