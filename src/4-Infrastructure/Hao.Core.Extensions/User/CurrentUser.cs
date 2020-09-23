using Hao.Library;
using Hao.Utility;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Hao.Core.Extensions
{
    public class CurrentUser : ICurrentUser
    {
        public long? Id { get; set; }
        
        public string Name { get; set; }
        
        public int? RoleLevel { get; set; }
        
        public string Jti { get; set; }
    }
}
