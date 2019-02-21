namespace BellumGens.Api
{
    public static class CORSConfig
    {
#if DEBUG
		public const string allowedOrigins = "http://localhost:4200";
#endif

#if !DEBUG
		public const string allowedOrigins = "https://bellumgens.com";
#endif

		public const string allowedHeaders = "*";
        public const string allowedMethods = "*";
    }
}