using JwtAuthServer.Core.Dtos;
using JwtAuthServer.Core.Models;
using JwtAuthServer.Core.Services;
using JwtAuthServer.Service.Mapping.AutoMapper;
using Microsoft.AspNetCore.Identity;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthServer.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserApp> _userManager;
        
        public UserService(UserManager<UserApp>userManager)
        {
            _userManager = userManager;
        }
        public async Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto)
        {
            var existUser = await _userManager.FindByEmailAsync(createUserDto.Email);
            if (existUser!=null)
            {
                return Response<UserAppDto>.Fail(new ErrorDto("Email kullanımda",true),400);
            }
            var createdUser = new UserApp();
            createdUser.Email = createUserDto.Email;
            createdUser.UserName = createUserDto.UserName;
            createdUser.City = createUserDto.City;
            var result = await _userManager.CreateAsync(createdUser, createUserDto.Password);
           
            if (!result.Succeeded)
            {
                return Response<UserAppDto>.Fail(new ErrorDto(result.Errors.Select(p => p.Description).ToList(), true),400);
            }
            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(createdUser), 200);
        }

        public async Task<Response<UserAppDto>> GetUserByUserNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user==null)
            {
                return Response<UserAppDto>.Fail(new ErrorDto("Kullanıcı adı bulunamadı!", true), 404);

            }
            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
        }
        public async Task<Response<NoDataDto>> AddToRoleAsync(string role,string userId)
        {
            var userApp = await _userManager.FindByIdAsync(userId);
            await _userManager.AddToRoleAsync(userApp, role);
            return Response<NoDataDto>.Success(200);
        }
    }
}
