namespace Identity.Client
{
    public static class Constants
    {
        public const string AuthorizationEndpoint = "https://localhost:6001/oauth/authorize";
        public const string TokenEndpoint = "https://localhost:6001/oauth/token";
        public const string CallbackPath = "/oauth/callback";
        public const string ClientSecret = "client_secret";
        public const string ClientId = "client_id";
        public const string ServerUrl = "https://localhost:7001/secret/index";
        public const string ApiUrl = "https://localhost:6001/secret/index";
    }
}
