using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Solid.Auth;
using Solid.Auth.Models;
using Solid.Auth.Models.CQRS.Commands;
using Solid.Auth.Services.Repository;
using System.Security.Claims;

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
            {
                return Unauthorized(new { message = "Invalid credentials" });

            }
            else
            {
                var claimsPrincipal = _userService.GetClaimsFromToken(tokenInfo.Token);
                // Extract specific claims (for example, CompanyID, UserID, etc.)
                var companyIdClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "CompanyID")?.Value;
                var userIdClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;

                var addtokenInfoCommand = new AddUserTokenLogCommand()
                {
                    CompanyID = Int32.Parse(companyIdClaim) ,
                    UserID = Int32.Parse(userIdClaim),
                    ExpiryDate = tokenInfo.ExpiresIn,
                    RefreshToken = tokenInfo.RefreshToken,

                };
                var response = await _userService.AddUserTokenLogAsync(addtokenInfoCommand);
            }
               
            

            return Ok(tokenInfo);
        }
    }
}
