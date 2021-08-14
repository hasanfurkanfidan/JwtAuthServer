using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthServer.Service.Helpers
{
    public static class JwtSecurityTokenHelper
    {
        public static JwtSecurityToken CreateJwtSecurityToken(string issuer,DateTime expiration,List<Claim>claims,SigningCredentials signingCredentials)
        {
            return new JwtSecurityToken(issuer: issuer, expires: expiration, notBefore: DateTime.Now, claims: claims, signingCredentials: signingCredentials);
        }
    }
}
