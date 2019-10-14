using System.Configuration;

namespace BellumGens.Api
{
	public class AppInfoDescriptior
	{
		public string steamApiKey;
		public string gameId;
		public string twitchClientId;
		public string twitchSecret;
		public string publicVapidKey;
		public string privateVapidKey;
		public string email;
		public string emailUsername;
		public string emailPassword;
        public string bank;
        public string bankAccountOwner;
        public string bic;
        public string bankAccount;
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
				gameId = ConfigurationManager.AppSettings["gameId"],
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
