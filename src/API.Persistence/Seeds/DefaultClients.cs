using API.Domain.Entities;
using System.Collections.Generic;

namespace API.Persistence.Seeds
{
    public class DefaultClients
    {
        public static IEnumerable<Client> ClientList() {
            return new List<Client>
            {
                new Client {
                    Id = 1,
                    DNI = "11111111",
                    FirstName = "Luis Eduardo",
                    LastName = "Cochachi Chamorro"                    
                },
                new Client {
                    Id = 2,
                    DNI = "11111112",
                    FirstName = "Euler",
                    LastName = "Gonzales Verde"
                },
                new Client {
                    Id = 3,
                    DNI = "11111113",
                    FirstName = "Felix",
                    LastName = "Agama Criollo"
                }
            };
        }
    }
}
