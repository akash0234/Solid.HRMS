using Solid.Auth.Models;
using Solid.Core.Models;

namespace Solid.Auth.Services.Repository
{
    public interface IUserService
    {
        Task<CommonResponseModel> RegisterUserAsync(RegisterRequestModel user);
        Task<JwtTokenInfo> Authenticate(LoginRequestModel loginRequest);
        //Task<UserModel> LoginUserAsync(string username, string password);   
    }
}
