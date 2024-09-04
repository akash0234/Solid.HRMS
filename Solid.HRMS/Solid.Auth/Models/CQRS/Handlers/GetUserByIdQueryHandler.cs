using Microsoft.Data.SqlClient;
using Solid.Auth.Models.CQRS.Queries;
using Solid.Auth.Services.Repository;
using Solid.Core.Services.Repository.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.Auth.Models.CQRS.Handlers
{
    public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserModel>
    {
        private readonly IUserRepository _userRepo;
        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            this._userRepo = userRepository;
        }


        public async Task<UserModel> ExecuteAsync(GetUserByIdQuery query)
        {
            try
            {
                return  await _userRepo.GetUserAsync(query);
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.Message);
                return null; 
            }
        }

       
    }

}
