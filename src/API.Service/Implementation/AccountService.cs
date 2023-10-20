using API.Domain.Auth;
using API.Domain.Common;
using API.Domain.Settings;
using API.Domain.Shared;
using API.Persistence.Seeds;
using API.Service.Contract;
using API.Service.Exceptions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Service.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly JWTSettings _jWTSetting;
        public AccountService(IOptions<JWTSettings> jwtSettings)
        {
            _jWTSetting = jwtSettings.Value;
        }
        public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
        {
            var user = DefaultUsers.UserList().Where(c => c.Email == request.Email).FirstOrDefault();

            if (user == null)
            {
                throw new ApiException($"No Accounts Registered with {request.Email}");
            }

            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);

            AuthenticationResponse authResponse = new AuthenticationResponse();
            authResponse.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authResponse.Email = user.Email;
            authResponse.UserName = user.UserName;

            return Response<AuthenticationResponse>.Success(authResponse, "OK");
        }
        private async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user)
        {
            // Basic Sample
            return await Task.Run<JwtSecurityToken>(() =>
            {
                string ipAddress = IpHelper.GetIpAddress();
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                    new Claim("ip",ipAddress)
                };

                var symetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jWTSetting.Key));
                var signingCredentials = new SigningCredentials(symetricSecurityKey, SecurityAlgorithms.HmacSha256);

                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: _jWTSetting.Issuer,
                    audience: _jWTSetting.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(_jWTSetting.DurationInMinutes),
                    signingCredentials: signingCredentials);

                return jwtSecurityToken;
            });
        }
    }
}
