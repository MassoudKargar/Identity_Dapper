namespace Server
{
    public static class Constants
    {
        public const string Issuer = Audiance;
#if DEBUG
        public const string Audiance = "https://localhost:44382";
#else
        public const string Audiance = "https://192.168.5.215:44382";
#endif
        public const string Secret = "not_too_short_secret_otherwise_it_might_error";
    }
}
