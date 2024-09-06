using Microsoft.IdentityModel.Tokens;
using Solid.Auth.Models;
using Solid.Auth.Models.CQRS.Commands;
using Solid.Core.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Solid.Auth.Services.Repository
{
    public interface IUserService
    {
        Task<CommonResponseModel> RegisterUserAsync(RegisterRequestModel user);
        Task<JwtTokenInfo> Authenticate(LoginRequestModel loginRequest);
        Task<CommonResponseModel> AddUserTokenLogAsync(AddUserTokenLogCommand tokenLog);
        ClaimsPrincipal GetClaimsFromToken(string token);
        
        //Task<UserModel> LoginUserAsync(string username, string password);   
    }
}
