namespace BellumGens.Api
{
	public static class CORSConfig
	{
#if DEBUG
		public const string allowedOrigins = "http://localhost:4200";
		public const string returnOrigin = "http://localhost:4200";
		public const string apiDomain = "https://localhost:44394";
#endif

#if !DEBUG
		public const string allowedOrigins = "https://bellumgens.com,http://staging.bellumgens.com,https://staging.bellumgens.com";
		public const string returnOrigin = "https://bellumgens.com";
		public const string apiDomain = "https://api.bellumgens.com";
#endif

		public const string allowedHeaders = "*";
		public const string allowedMethods = "*";
	}
}