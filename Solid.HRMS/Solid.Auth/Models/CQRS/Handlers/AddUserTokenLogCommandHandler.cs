using Solid.Auth.Models.CQRS.Commands;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solid.Core.Services.Repository;
using Solid.Core.Services.Repository.CQRS;
using Solid.Auth.Services.Repository;


namespace Solid.Auth.Models.CQRS.Handlers
{
    public class AddUserTokenLogCommandHandler : ICommandHandler<AddUserTokenLogCommand>
    {
        private readonly IUserRepository _userRepo;
        public AddUserTokenLogCommandHandler(IUserRepository userRepository)
        {
            this._userRepo = userRepository;
        }



        public async Task ExecuteAsync(AddUserTokenLogCommand command)
        {
            try
            {
                await _userRepo.AddUserTokenLogAsync(command);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
   

}
