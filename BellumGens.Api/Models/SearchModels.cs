using System;

namespace BellumGens.Api.Models
{
	public class TeamSearchModel
	{
		public PlaystyleRole? role { get; set; }
		public double overlap { get; set; }
	}

	public class PlayerSearchModel
	{
		public PlaystyleRole? role { get; set; }
		public double overlap { get; set; }
		public Guid? teamId { get; set; }
	}
}