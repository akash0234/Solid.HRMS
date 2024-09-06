using Solid.Core.Services.Repository.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Solid.Auth.Models.CQRS.Commands
{
    
    public class AddUserTokenLogCommand : ICommand
    {
     
        public int UserID { get; set; }             // ID of the user (foreign key)
        public string RefreshToken { get; set; }    // JWT Refresh token
        public int CompanyID { get; set; }          // ID of the company (foreign key)
        public DateTime ExpiryDate { get; set; }    // Expiration date of the access token
    }
}
