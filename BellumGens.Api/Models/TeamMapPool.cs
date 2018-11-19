using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Models
{
	public class TeamMapPool
	{
		[Key]
		[Column(Order = 0)]
		public Guid TeamId { get; set; }

		[Key]
		[Column(Order = 1)]
		public CSGOMaps Map { get; set; }
		public bool IsPlayed { get; set; }

		[JsonIgnore]
		[ForeignKey("TeamId")]
		public virtual CSGOTeam Team { get; set; }
	}
}