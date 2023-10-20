using API.Domain.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Persistence.Seeds
{
    public static class DefaultUsers
    {
        public static List<ApplicationUser> UserList()
        {
            return new List<ApplicationUser>
            {
                new ApplicationUser{                     
                    UserName = "basicuser",
                    Email = "basicuser@vaetech.net"
                }
            };
        }
    }
}
