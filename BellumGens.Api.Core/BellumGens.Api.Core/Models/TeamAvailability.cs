using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellumGens.Api.Core.Models
{
	public class TeamAvailability : Availability
	{
		[Key]
		[Column(Order = 0)]
		public Guid TeamId { get; set; }

		[Key]
		[Column(Order = 1)]
		public DayOfWeek Day { get; set; }

		[JsonIgnore]
		[ForeignKey("TeamId")]
		public virtual CSGOTeam Team { get; set; }
	}
}