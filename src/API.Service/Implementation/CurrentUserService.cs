using API.Service.Contract;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace API.Service.Implementation
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            Username = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            Email = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
        }
        public string Username { get; }
        public string Email { get; }
    }
}
