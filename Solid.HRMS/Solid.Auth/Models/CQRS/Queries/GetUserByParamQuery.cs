using Solid.Core.Services.Repository.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.Auth.Models.CQRS.Queries
{
    public class GetUserByIdQuery : IQuery<UserModel>
    {
        public int UserID { get; set; }
    }
    
}
