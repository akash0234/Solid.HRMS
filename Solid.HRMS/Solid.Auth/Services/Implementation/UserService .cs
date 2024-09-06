using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Solid.Auth.Models;
using Solid.Auth.Models.CQRS.Commands;
using Solid.Auth.Models.CQRS.Handlers;
using Solid.Auth.Models.CQRS.Queries;
using Solid.Auth.Services.Repository;
using Solid.Core.Models;
using Solid.Core.Services.Repository;
using Solid.Core.Services.Repository.CQRS;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace Solid.Auth.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly ICommandHandler<AddUserCommand> _addUserCommandHandler;
        private readonly ICommandHandler<AddUserTokenLogCommand> _addUserTokenCommandHandler;
        private readonly ICommandHandler<LoginUserCommand, UserModel> _loginUserCommandHandler;
        private readonly IQueryHandler<GetUserByEmailQuery, UserModel> _userQueryHandler;
        private readonly IConfiguration _configuration;

        public UserService(ICommandHandler<AddUserCommand> addUserCommandHandler,
            ICommandHandler<AddUserTokenLogCommand> addUserTokenCommandHandler,
            ICommandHandler<LoginUserCommand,UserModel> loginUserCommandHandler,
            IQueryHandler<GetUserByEmailQuery, UserModel> userQueryHandler,
            IConfiguration configuration)
        {
            _addUserCommandHandler = addUserCommandHandler;
            _addUserTokenCommandHandler = addUserTokenCommandHandler;
            _loginUserCommandHandler = loginUserCommandHandler;
            _userQueryHandler = userQueryHandler;
            this._configuration = configuration;
        }

        public async Task<CommonResponseModel> RegisterUserAsync(RegisterRequestModel user)
        {
            // Implementation for registering a user
            try
            {
                // Generate Salt and Hash the password using PBKDF2
                
                PasswordHashModel? generatedHashPasssword = HashingUtility.GenerateHashPassword(user.Password);

                var existingUser = await _userQueryHandler.ExecuteAsync(new GetUserByEmailQuery() { UserEmail = user.EmailID });
                if(existingUser is not null)
                {
                    return new CommonResponseModel() { IsSuccess = false, Message = "Email Already Exist" };
                }
                var dbuser = new AddUserCommand()
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    Email = user.EmailID,
                    LastName = user.LastName,
                    OrganisationID = user.OrganisationID.GetValueOrDefault(),
                    PasswordHash = generatedHashPasssword.PasswordHash,
                    RoleIDs = "3",//Sending RoleID 3 for User
                    Salt = generatedHashPasssword.PasswordSalt,
                    
                };

                await _addUserCommandHandler.ExecuteAsync(dbuser);
                return new CommonResponseModel() { IsSuccess = true,Message = "User Registered Successfully"};
            }
            catch (Exception ex) { return new CommonResponseModel() { IsSuccess = false, Message = "User Registration Failed" }; }
            
        }
        public async Task<CommonResponseModel> AddUserTokenLogAsync(AddUserTokenLogCommand tokenLog)
        {
            try
            {
                await _addUserTokenCommandHandler.ExecuteAsync(tokenLog);

                return new CommonResponseModel() { IsSuccess = true, Message = "Token log added successfully" };
            }
            catch (Exception ex)
            {
                return new CommonResponseModel() { IsSuccess = false, Message = "Failed to add token log" };
            }
        }
        public async Task<JwtTokenInfo> Authenticate(LoginRequestModel loginRequest)
        {
            // Validate user credentials against the database
            var loginreq = new LoginUserCommand()
            {
                UserName = loginRequest.UserName,
                Password = loginRequest.Password,
            };
            var response = await _loginUserCommandHandler.ExecuteAsync(loginreq);
            if(response is not null)
            {
                var token = GenerateJwtToken(response);
                var refreshToken = GenerateRefreshToken();

                // Save the refresh token in the database against the user
                


                return new JwtTokenInfo() { Token = token, RefreshToken = refreshToken, ExpiresIn = DateTime.UtcNow.AddMinutes(30) };
            }



            return null;
        }




        public ClaimsPrincipal GetClaimsFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            if (tokenHandler.CanReadToken(token))
            {
                var jwtToken = tokenHandler.ReadJwtToken(token);

                // Extract claims
                var claims = jwtToken.Claims;

                var identity = new ClaimsIdentity(claims, "jwt");
                var principal = new ClaimsPrincipal(identity);

                return principal;
            }

            throw new SecurityTokenException("Invalid token");
        }
        private string GenerateJwtToken(UserModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Convert.ToString(_configuration["JwtSettings:Secret"]));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Email, user.UserEmail),
                new Claim("CompanyID", user.OrganisationID.ToString()),
                // Add more claims as needed
            };

            // Add roles to claims
            claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.RoleName)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30), // Token expiry time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        //public async Task<UserModel> LoginUserAsync(string username, string password)
        //{
        //    // Implementation for logging in a user
        //    return new UserModel();
        //}
    }
}
