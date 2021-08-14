using JwtAuthServer.Core.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthServer.Service.Extentions
{
    public static class ClaimsExtention
    {
        public static void SetName(this List<Claim> claims,string name)
        {
            claims.Add(new Claim(ClaimTypes.Name, name));
        }
        public static void SetNameIdentifier(this List<Claim>claims,string id)
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, id));
        }
        public static void SetEmail(this List<Claim>claims,string email)
        {
            claims.Add(new Claim(ClaimTypes.Email, email));
        }
        public static void SetRoles(this List<Claim> claims, string[] roles)
        {
            roles.ToList().ForEach(p => claims.Add(new Claim(ClaimTypes.Role, p)));
        }
        public static void SetAudiences(this List<Claim> claims, string[] audiences)
        {
            audiences.ToList().ForEach(audience => claims.Add(new Claim(JwtRegisteredClaimNames.Aud, audience)));
        }
        public static void SetJti(this List<Claim>claims,string jti)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, jti));
        }
        public static void SetSub(this List<Claim>claims,string sub)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, sub));
        }
        public static void SetAudiencesByClient(this List<Claim> claims,Client client)
        {
            client.Audiences.ForEach(audience => claims.Add(new Claim(JwtRegisteredClaimNames.Aud, audience)));
        }
    }
}
