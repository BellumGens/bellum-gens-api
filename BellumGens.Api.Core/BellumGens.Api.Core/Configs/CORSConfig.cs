namespace BellumGens.Api.Core
{
	public static class CORSConfig
	{
#if DEBUG
		public const string allowedOrigins = "http://localhost:4200,http://localhost:4000,http://localhost:4201,http://localhost:4001";
		public const string returnOrigin = "http://localhost:4200";
		public const string apiDomain = "https://localhost:44394";
#endif

#if !DEBUG
		public const string allowedOrigins = "https://bellumgens.com,https://www.bellumgens.com,https://eb-league.com,https://www.eb-league.com,http://staging.bellumgens.com,http://staging.eb-league.com";
		public const string returnOrigin = "https://bellumgens.com";
		public const string apiDomain = "https://api.bellumgens.com";
#endif

        public const string allowedHeaders = "*";
		public const string allowedMethods = "*";
	}
}