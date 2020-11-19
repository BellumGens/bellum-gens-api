using Microsoft.Extensions.Configuration;

namespace BellumGens.Api.Core
{
	public class AppKeysDescriptior
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

	public class AppConfiguration
	{
		private AppKeysDescriptior _config;

		public AppConfiguration(IConfiguration configuration)
        {
			_config = new AppKeysDescriptior()
			{
				steamApiKey = configuration["steamApiKey"],
				battleNetClientId = configuration["battleNetClientId"],
				battleNetClientSecret = configuration["battleNetClientSecret"],
				twitchClientId = configuration["twitchClientId"],
				twitchSecret = configuration["twitchSecret"],
				publicVapidKey = configuration["publicVapidKey"],
				privateVapidKey = configuration["privateVapidKey"],
				email = configuration["email"],
				emailUsername = configuration["emailUsername"],
				emailPassword = configuration["emailPassword"],
				bank = configuration["bank"],
				bankAccountOwner = configuration["bankAccountOwner"],
				bic = configuration["bic"],
				bankAccount = configuration["bankAccount"]
			};
		}

		public AppKeysDescriptior Config
		{
			get
			{
				return _config;
			}
		}
	}
}
