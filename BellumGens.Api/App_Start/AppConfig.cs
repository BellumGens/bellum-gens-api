using System.Configuration;

namespace BellumGens.Api
{
	public class AppInfoDescriptior
	{
		public string steamApiKey { get; set; }
		public string battleNetClientId { get; set; }
		public string battleNetClientSecret { get; set; }
		public string csgoGameId { get; set; } = "730";
		public string twitchClientId { get; set; }
		public string twitchSecret { get; set; }
		public string publicVapidKey { get; set; }
		public string privateVapidKey { get; set; }
		public string email { get; set; }
		public string emailUsername { get; set; }
		public string emailPassword { get; set; }
        public string bank { get; set; }
        public string bankAccountOwner { get; set; }
        public string bic { get; set; }
        public string bankAccount { get; set; }
	}

	public static class AppInfo
	{
		private static AppInfoDescriptior _steamInfo;

		public static AppInfoDescriptior Config
		{
			get
			{
				if (_steamInfo == null)
				{
					_steamInfo = Initialize();
				}
				return _steamInfo;
			}
		}

		private static AppInfoDescriptior Initialize()
		{
			return new AppInfoDescriptior()
			{
				steamApiKey = ConfigurationManager.AppSettings["steamApiKey"],
				battleNetClientId = ConfigurationManager.AppSettings["battleNetClientId"],
				battleNetClientSecret = ConfigurationManager.AppSettings["battleNetClientSecret"],
				csgoGameId = ConfigurationManager.AppSettings["gameId"],
				twitchClientId = ConfigurationManager.AppSettings["twitchClientId"],
				twitchSecret = ConfigurationManager.AppSettings["twitchSecret"],
				publicVapidKey = ConfigurationManager.AppSettings["publicVapidKey"],
				privateVapidKey = ConfigurationManager.AppSettings["privateVapidKey"],
				email = ConfigurationManager.AppSettings["email"],
				emailUsername = ConfigurationManager.AppSettings["emailUsername"],
				emailPassword = ConfigurationManager.AppSettings["emailPassword"],
                bank = ConfigurationManager.AppSettings["bank"],
                bankAccountOwner = ConfigurationManager.AppSettings["bankAccountOwner"],
                bic = ConfigurationManager.AppSettings["bic"],
                bankAccount = ConfigurationManager.AppSettings["bankAccount"]
            };
		}

	}
}
