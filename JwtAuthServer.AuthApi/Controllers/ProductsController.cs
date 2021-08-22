using JwtAuthServer.Core.Dtos;
using JwtAuthServer.Core.Models;
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
    public class ProductsController : ControllerBase
    {
        private readonly IGenericService<Product, ProductDto> _productService;
        public ProductsController(IGenericService<Product, ProductDto> productService)
        {
            _productService = productService;
        }
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _productService.GetAllAsync();
            return Ok(response);
        }
    }
}
