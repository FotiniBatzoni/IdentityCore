using Newtonsoft.Json;

namespace IdentityCore.Authorization
{
    public class JwtToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_at")]
        public DateTime ExpirisesAt { get; set; }
    }
}
