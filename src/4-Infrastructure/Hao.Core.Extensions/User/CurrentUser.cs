using Hao.Library;
using Hao.Utility;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Hao.Core.Extensions
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private long? _id;
        /// <summary>
        ///用户id
        /// </summary>
        public long? Id
        {
            get => _httpContextAccessor?.HttpContext == null ? null : H_Convert.ToLongOrNull(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sid)?.Value);
            set { _id = value; }
        }
        
        private string _name;
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name
        {
            get => _httpContextAccessor?.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == H_ClaimsName.Name)?.Value.ToString();
            set { _name = value; }
        }

        private int? _roleLevel;
        /// <summary>
        /// 用户角色等级
        /// </summary>
        public int? RoleLevel
        {
            get => _httpContextAccessor?.HttpContext == null ? null : H_Convert.ToIntOrNull(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == H_ClaimsName.RoleLevel)?.Value);
            set { _roleLevel = value; }
        }

        private string _jti;
        /// <summary>
        /// json web token唯一标识
        /// </summary>
        public string Jti 
        {
            get
            {
                return _httpContextAccessor?.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;
            }
            set { _jti = value; }
        }

        ///// <summary>
        /////用户编号 
        ///// </summary>
        //public long? Id
        //{
        //    get => _httpContext == null ? -1 : HConvert.ToLong(_httpContext.Session.GetString("CurrentUser_UserId"));
        //    set => _httpContext.Session.SetString("CurrentUser_UserId", value.ToString());
        //}

        ///// <summary>
        ///// 用户姓名
        ///// </summary>
        //public string Name
        //{
        //    get => _httpContext == null ? "系统" : _httpContext.Session.GetString("CurrentUser_UserName");
        //    set => _httpContext.Session.SetString("CurrentUser_UserName", value);
        //}
    }
}
