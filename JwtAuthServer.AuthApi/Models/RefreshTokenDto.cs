using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuthServer.AuthApi.Models
{
    public class RefreshTokenDto
    {
        public string Code { get; set; }
    }
}
