using System.Text.Json.Serialization;

namespace APP.Models
{
    public class AuthenticationResponse
    {        
        public string UserName { get; set; }        
        public string Email { get; set; }        
        public string JWToken { get; set; }
    }
}
