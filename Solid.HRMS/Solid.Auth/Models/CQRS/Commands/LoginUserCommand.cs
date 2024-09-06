using Solid.Core.Services.Repository.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Solid.Auth.Models.CQRS.Commands
{
    public class LoginUserCommand : ICommand<UserModel>
    {
        public string UserName { get; set; }
        public string Password { get; set; }

    }
}
