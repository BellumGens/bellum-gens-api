using System.Configuration;

namespace BellumGens.Api
{
	public class SteamInfoDescriptior
	{
		public string steamApiKey;
		public string gameId;
	}

	public static class SteamInfo
	{
		private static SteamInfoDescriptior _steamInfo;

		public static SteamInfoDescriptior Config
		{
			get
			{
				return SteamInfo._steamInfo;
			}
		}

		public static void Initialize()
		{
			if (SteamInfo._steamInfo == null)
            {
                SteamInfo._steamInfo = new SteamInfoDescriptior()
                {
                    steamApiKey = ConfigurationManager.AppSettings["steamApiKey"],
                    gameId = ConfigurationManager.AppSettings["gameId"]
                };
            }
		}

	}
}
