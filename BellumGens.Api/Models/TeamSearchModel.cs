namespace BellumGens.Api.Models
{
	public class TeamSearchModel
	{
		public string name { get; set; }
		public PlaystyleRole? role { get; set; }
		public double scheduleOverlap { get; set; }
	}
}