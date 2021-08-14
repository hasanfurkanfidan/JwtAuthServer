using JwtAuthServer.Core.Configuration;
using JwtAuthServer.Core.Dtos;
using JwtAuthServer.Core.Models;
using JwtAuthServer.Core.Services;
using JwtAuthServer.Service.Extentions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SharedLibrary.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthServer.Service.Services
{
    public class TokenService : ITokenService
    {
        private readonly CustomTokenOption _tokenOption;
        private readonly UserManager<UserApp> _userManager;
        public TokenService(UserManager<UserApp> userManager, IOptions<CustomTokenOption> options)
        {
            _userManager = userManager;
            _tokenOption = options.Value;
        }
        private string CreateRefreshToken()
        {
            var numberByte = new Byte[32];
            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(numberByte);
            }
            return Convert.ToBase64String(numberByte);
        }
        public TokenDto CreateToken(UserApp userApp)
        {
            throw new NotImplementedException();
        }

        public ClientTokenDto CreateClientTokenDto(Client client)
        {
            throw new NotImplementedException();
        }
        private List<Claim> SetClaims(UserApp userApp, List<string> audiences)
        {
            var claims = new List<Claim>();
            var roles = _userManager.GetRolesAsync(userApp).Result.ToArray();
            claims.SetName(userApp.UserName);
            claims.SetEmail(userApp.Email);
            claims.SetNameIdentifier(userApp.Id);
            claims.SetRoles(roles);
            claims.SetAudiences(audiences.ToArray());
            claims.SetJti(Guid.NewGuid().ToString());
            return claims;
        }

        private List<Claim>GetClaimsByClient(Client client)
        {
            var claims = new List<Claim>();
            claims.SetAudiencesByClient(client);
            claims.SetJti(Guid.NewGuid().ToString());
            claims.SetSub(client.ClientId);
            return claims;
        }
    }
}
