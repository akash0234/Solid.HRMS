using Microsoft.Data.SqlClient;
using Solid.Auth.Models;
using Solid.Auth.Models.CQRS.Commands;
using Solid.Auth.Models.CQRS.Queries;
using Solid.Auth.Services.Repository;
using Solid.Core.Helpers;
using Solid.Core.Services.Repository;

namespace Solid.Auth.Services.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly IDMLServices _dmlServices;

        public UserRepository(IDMLServices dmlServices)
        {
            _dmlServices = dmlServices;
        }

        public async Task AddUserAsync(AddUserCommand user)
        {
            var response = await _dmlServices.ExecuteStoredProcedureNonQueryAsync(StoreProcedures.InsertUpdateUser, user, "ReturnID");
        }
        public async Task AddUserTokenLogAsync(AddUserTokenLogCommand user)
        {
            var response = await _dmlServices.ExecuteStoredProcedureNonQueryAsync(StoreProcedures.InsertUserTokenLog, user);
        }
        public async Task<UserModel> GetUserAsync(GetUserByEmailQuery query)
        {
            try
            {
                var userDST = await _dmlServices.GetDataSet(StoreProcedures.GetUserByEmailID, query);
                if (userDST != null && userDST.Tables.Count > 0)
                {
                    var userModel = new UserModel();
                    //Map User Details
                    var userDetailsModel = DataTableExtensions.ConvertToList<UserModel>(userDST.Tables[0]);
                    userModel = userDetailsModel.FirstOrDefault();
                    //Map Roles
                    var userRoleDetailsModel = DataTableExtensions.ConvertToList<UserRole>(userDST.Tables[1]);
                    userModel.Roles = new List<UserRole>(userRoleDetailsModel);
                    return userModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }
        public async Task<UserModel> GetUserAsync(GetUserByIdQuery query)
        {
            var userDST = await _dmlServices.GetDataSet(StoreProcedures.GetUserByID, query);
            if (userDST != null && userDST.Tables.Count > 0)
            {
                var userModel = new UserModel();
                //Map User Details
                var userDetailsModel = DataTableExtensions.ConvertToList<UserModel>(userDST.Tables[0]);
                userModel = userDetailsModel.FirstOrDefault();
                //Map Roles
                var userRoleDetailsModel = DataTableExtensions.ConvertToList<UserRole>(userDST.Tables[1]);
                userModel.Roles = new List<UserRole>(userRoleDetailsModel);
                return userModel;
            }
            else
            {
                return default;
            }


        }
        public async Task<UserModel> ValidateUserAsync(LoginUserCommand command)
        {
            var checkUser = await GetUserAsync(new GetUserByEmailQuery() { UserEmail = command.UserName });
            if (checkUser == null)
            {
                return null;
            }
            else
            {
                // Define parameters for the stored procedure
                var emailParam = new { UserEmail = command.UserName };

                // Execute stored procedure to get user details and salt
                var userDST = await _dmlServices.GetDataTable(StoreProcedures.GetHasSaltByID, emailParam);

                if (userDST != null && userDST.Rows.Count > 0)
                {
                    var passwordHashModel = new PasswordHashModel()
                    {
                        PasswordHash = Convert.ToString(userDST.Rows[0]["PasswordHash"]),
                        PasswordSalt = Convert.ToString(userDST.Rows[0]["Salt"])
                    };



                    // Verify the password
                    var isPasswordValid = HashingUtility.VerifyPassword(passwordHashModel, command.Password);
                    if (isPasswordValid)
                    {
                        

                        return checkUser;
                    }
                    else
                    {
                        return null;
                    }

                }
                else
                {
                    // No data found
                    return null;
                }
            }


            
        }

    }
}
