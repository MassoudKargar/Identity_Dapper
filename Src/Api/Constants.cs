namespace Api
{
    public static class Constants
    {
#if DEBUG
        public const string ServerUrl = "https://localhost:44382/oauth/validate";
#else
        public const string ServerUrl = "https://192.168.5.215:44382/oauth/validate";
#endif
        }
}
