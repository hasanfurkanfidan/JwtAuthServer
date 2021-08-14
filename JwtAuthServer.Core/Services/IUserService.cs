using JwtAuthServer.Core.Dtos;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthServer.Core.Services
{
    public interface IUserService
    {
        Task<Response<UserAppDto>> GetUserByUserNameAsync(string userName);
        Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto);
    }
}
