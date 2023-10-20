using System.Text.Json.Serialization;

namespace APP.Models
{
    public class ClientModel
    {        
        public int Id { get; set; }        
        public string FirstName { get; set; }        
        public string LastName { get; set; }        
        public string DNI { get; set; }
        
        public string FullName => $"{FirstName} {LastName}";
    }
}
