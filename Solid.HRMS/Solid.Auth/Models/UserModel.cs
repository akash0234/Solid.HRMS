using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.Auth.Models
{
    public class UserModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserEmail { get; set; }
        public List<UserRole> Roles { get; set; }

        public int OrganisationID { get;set; }
    }
    public class UserRole
    {
        public string RoleName { get; set; }
    }
}
