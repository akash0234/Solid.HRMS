using Solid.Auth.Models;
using Solid.Auth.Models.CQRS.Commands;
using Solid.Auth.Models.CQRS.Queries;

namespace Solid.Auth.Services.Repository
{
    public interface IUserRepository
    {
        /// <summary>
        ///  Add User Into Database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task AddUserAsync(AddUserCommand user);
        /// <summary>
        /// Get User By User ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<UserModel> GetUserAsync(GetUserByIdQuery query);
        /// <summary>
        /// Get User By Email ID
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<UserModel> GetUserAsync(GetUserByEmailQuery query);
        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<UserModel> ValidateUserAsync(LoginUserCommand command);
        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task AddUserTokenLogAsync(AddUserTokenLogCommand user);
        //Task<User> GetUserByUsernameAsync(string username);
    }
}
