using System.ComponentModel.DataAnnotations.Schema;

namespace API.Domain.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DNI { get; set; }
        
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}
