using JwtAuthServer.Core.Configuration;
using JwtAuthServer.Core.Dtos;
using JwtAuthServer.Core.Models;
using JwtAuthServer.Core.Services;
using JwtAuthServer.Service.Extentions;
using JwtAuthServer.Service.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SharedLibrary.Configurations;
using SharedLibrary.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
        public  TokenDto CreateToken(UserApp userApp)
        {
            //Getting accestokenexpiration
            var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.AccessTokenExpiration);
            //Getting refreshtokenexpiration

            var refreshTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.RefreshTokenExpiration);
            //Getting symetric security key

            var securityKey = SignService.GetSymetricSecurityKey(_tokenOption.SecurityKey);
            //Getting signing credential
            var signInCredential = SignInCredentialHelper.GetSignInCredential(securityKey);

            var token = JwtSecurityTokenHelper.CreateJwtSecurityToken(_tokenOption.Issuer, accessTokenExpiration, SetClaims(userApp, _tokenOption.Audience), signInCredential);

            var handler = new JwtSecurityTokenHandler();

            var jwtToken = handler.WriteToken(token);
            return new TokenDto
            {
                AccessToken = jwtToken,
                AccessTokenExpiration = accessTokenExpiration,
                RefreshToken = CreateRefreshToken(),
                RefreshTokenExpiration = refreshTokenExpiration
            };
        }

        public ClientTokenDto CreateClientTokenDto(Client client)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.AccessTokenExpiration);
            //Getting symetric security key
            var securityKey = SignService.GetSymetricSecurityKey(_tokenOption.SecurityKey);
            //Getting signing credential
            var signInCredential = SignInCredentialHelper.GetSignInCredential(securityKey);

            var token = JwtSecurityTokenHelper.CreateJwtSecurityToken(_tokenOption.Issuer, accessTokenExpiration, GetClaimsByClient(client), signInCredential);

            var handler = new JwtSecurityTokenHandler();

            var jwtToken = handler.WriteToken(token);
            return new ClientTokenDto
            {
                AccessToken = jwtToken,
                AccessTokenExpiration = accessTokenExpiration,
               
            };
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

        private List<Claim> GetClaimsByClient(Client client)
        {
            var claims = new List<Claim>();
            claims.SetAudiencesByClient(client);
            claims.SetJti(Guid.NewGuid().ToString());
            claims.SetSub(client.ClientId);
            return claims;
        }
    }
}
