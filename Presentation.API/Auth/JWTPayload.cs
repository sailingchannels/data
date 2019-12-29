
using System;

namespace Presentation.API.Auth
{
    public class JWTPayloadCredentials
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string scope { get; set; }
        public string token_type { get; set; }
        public long expiry_date { get; set; }
    }

    public class JWTPayload
    {
        public string _id { get; set; }
        public long iat { get; set; }
        public string country { get; set; }
        public string thumbnail { get; set; }
        public string title { get; set; }
        public DateTime lastLogin { get; set; }
        public JWTPayloadCredentials credentials { get; set; }
    }
}
