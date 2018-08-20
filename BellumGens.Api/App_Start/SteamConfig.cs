using Newtonsoft.Json;

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

		public static void Initialize(string path)
		{
			if (SteamInfo._steamInfo == null)
				SteamInfo._steamInfo = JsonConvert.DeserializeObject<SteamInfoDescriptior>(System.IO.File.ReadAllText(path));
		}

	}
}
