using System.Text.Json.Serialization;

namespace API.Domain.Auth
{
    public class AuthenticationResponse
    {        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string UserName { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Email { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string JWToken { get; set; }        
    }
}
