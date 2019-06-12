using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BellumGens.Api.Models
{
	public class TeamStrategy
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public Guid TeamId { get; set; }

		public string UserId { get; set; }

		public Side Side { get; set; }

		public string Title { get; set; }

		public CSGOMaps Map { get; set; }

		public string Description { get; set; }

		public string Url { get; set; }

		public string Image { get; set; }

		public string EditorMetadata { get; set; }

		public bool Visible { get; set; } = true;

		public string PrivateShareLink { get; set; }

		[NotMapped]
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public int Rating
		{
			get
			{
				return Votes.Where(v => v.Vote == VoteDirection.Up).Count() - Votes.Where(v => v.Vote == VoteDirection.Down).Count();
			}
			private set { }
		}

		[JsonIgnore]
		public virtual ICollection<StrategyVotes> Votes { get; set; }

		public virtual ICollection<StrategyComment> Comments { get; set; }

		[JsonIgnore]
		[ForeignKey("TeamId")]
		public virtual CSGOTeam Team { get; set; }

		[JsonIgnore]
		[ForeignKey("UserId")]
		public virtual ApplicationUser User { get; set; }
	}

	public enum Side
	{
		TSide,
		CTSide
	}
}