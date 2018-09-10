namespace BellumGens.Api.Models
{
	public enum PlaystyleRole
	{
		NotSet,
		IGL,
		Awper,
		EntryFragger,
		Support,
		Lurker
	}

	public class Role
	{
		public PlaystyleRole Id;
		public string Name;
	}
}