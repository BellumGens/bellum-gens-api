using System;

namespace BellumGens.Api.Models
{
	public class TeamSearchModel
	{
		public string name { get; set; }
		public PlaystyleRole? role { get; set; }
		public double scheduleOverlap { get; set; }
	}

	public class PlayerSearchModel
	{
		public string name { get; set; }
		public PlaystyleRole? role { get; set; }
		public double scheduleOverlap { get; set; }
		public Guid? teamId { get; set; }
	}
}