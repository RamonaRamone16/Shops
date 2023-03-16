using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.BLL.Services;
using Shop.Models.Dtos;

namespace Shop.API.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _service;

        public AuthController(AuthService service)
        {
            _service = service;
        }

        /// <summary>
        /// User registration
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(IdentityResult))]
        public async Task<IActionResult> RegisterAsync(UserRegistrationDto dto)
        {
            var result = await _service.CreateUserAsync(dto);
            return !result.Succeeded ? new BadRequestObjectResult(result) : StatusCode(201);
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IdentityResult))]
        public async Task<IActionResult> Login(UserLoginDto dto) 
        {
            var result = await _service.ValidateUserAsync(dto);
            return result ? Ok(new { Token = await _service.CreateTokenAsync() }) : Unauthorized();
        }
    }
}
