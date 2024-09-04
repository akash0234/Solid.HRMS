using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Solid.Auth;
using Solid.Auth.Models;
using Solid.Auth.Services.Repository;

namespace Solid.Authentication.Mcs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the username or email already exists
            //if (await _userService.UserExistsAsync(model.UserName, model.Email))
            //{
            //    return Conflict("A user with the same username or email already exists.");
            //}
            //
            
            // Add the user to the database
            var response = await _userService.RegisterUserAsync(model);
            if (response.IsSuccess)
            {
                return Ok(response);

            }
            else
                return BadRequest(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel loginRequest)
        {
            var tokenInfo = await _userService.Authenticate(loginRequest);

            if (tokenInfo == null)
                return Unauthorized(new { message = "Invalid credentials" });

            return Ok(tokenInfo);
        }
    }
}
