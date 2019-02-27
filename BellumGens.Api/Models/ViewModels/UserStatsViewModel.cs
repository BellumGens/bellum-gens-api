using SteamModels;
using SteamModels.CSGO;
using System.Collections.Generic;

namespace BellumGens.Api.Models
{
	public class UserStatsViewModel
	{
		private static List<Role> _roles = new List<Role>()
		{
			new Role()
			{
				Id = PlaystyleRole.NotSet,
				Name = "---"
			},
			new Role()
			{
				Id = PlaystyleRole.IGL,
				Name = "IGL"
			},
			new Role()
			{
				Id = PlaystyleRole.Awper,
				Name = "Awper"
			},
			new Role()
			{
				Id = PlaystyleRole.EntryFragger,
				Name = "Entry Fragger"
			},
			new Role()
			{
				Id = PlaystyleRole.Support,
				Name = "Support"
			},
			new Role()
			{
				Id = PlaystyleRole.Lurker,
				Name = "Lurker"
			}
		};

		public SteamUser steamUser;
		public string steamUserException;
		public CSGOPlayerStats userStats;
		public string userStatsException;
		public ICollection<UserAvailability> availability;
		public PlaystyleRole primaryRole;
		public PlaystyleRole secondaryRole;
		public ICollection<UserMapPool> mapPool;
		public ICollection<CSGOTeam> teams;
		public bool registered = false;
		public List<Role> roles {
			get
			{
				return UserStatsViewModel._roles;
			}
		}
	}
}