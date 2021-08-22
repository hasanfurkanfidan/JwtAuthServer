using JwtAuthServer.AuthApi.Models;
using JwtAuthServer.Core.Services;
using Microsoft.AspNetCore.Authorization;
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
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var response = await _userService.GetUserByUserNameAsync(HttpContext.User.Identity.Name);
            return Ok(response);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToRole(AddToRoleDto model)
        {
            var user = await _userService.GetUserByUserNameAsync(HttpContext.User.Identity.Name);
            var response = await _userService.AddToRoleAsync(model.Role,user.Data.Id);
            return Ok(response);
        }
    }
}
