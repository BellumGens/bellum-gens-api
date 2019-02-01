using System;

namespace BellumGens.Api.Models
{
	public class TeamSearchModel
	{
		public PlaystyleRole? role { get; set; }
		public double scheduleOverlap { get; set; }
	}

	public class PlayerSearchModel
	{
		public PlaystyleRole? role { get; set; }
		public double scheduleOverlap { get; set; }
		public Guid? teamId { get; set; }
	}
}