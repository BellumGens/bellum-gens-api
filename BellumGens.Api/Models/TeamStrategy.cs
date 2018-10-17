using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Models
{
	public class TeamStrategy
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public Guid TeamId { get; set; }

		public Side Side { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public string Url { get; set; }

		[JsonIgnore]
		[ForeignKey("TeamId")]
		public virtual CSGOTeam Team { get; set; }
	}

	public enum Side
	{
		TSide,
		CTSide
	}
}