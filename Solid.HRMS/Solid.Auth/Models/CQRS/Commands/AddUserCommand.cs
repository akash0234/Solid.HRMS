using Solid.Core.Services.Repository.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Solid.Auth.Models.CQRS.Commands
{
    public class AddUserCommand : ICommand
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
      
        public string RoleIDs { get; set; } // Comma separated Role
        public byte[] PasswordHash { get; set; }
        public byte[] Salt { get; set; }
        public int OrganisationID { get; set; }
    }
}
