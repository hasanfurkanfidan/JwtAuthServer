using JwtAuthServer.AuthApi.Models;
using JwtAuthServer.Core.Dtos;
using JwtAuthServer.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtAuthServer.AuthApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        public AuthController(IUserService userService,IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult>SignUp(CreateUserDto model)
        {
            var response = await _userService.CreateUserAsync(model);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult>CreateToken(LoginDto model)
        {
            var response = await _authenticationService.CreateTokenAsync(model);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult>CreateTokenByClient(ClientLoginDto model)
        {
            var response =  _authenticationService.CreateTokenByClient(model);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult>RevokeRefreshToken(string refreshToken)
        {
            var response = await _authenticationService.RevokeRefreshToken(refreshToken);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult>CreateTokenByRefreshToken(RefreshTokenDto model)
        {
            var response = await _authenticationService.CreateTokenByRefreshToken(model.Code);
            return Ok(response);
        }
    }
}
