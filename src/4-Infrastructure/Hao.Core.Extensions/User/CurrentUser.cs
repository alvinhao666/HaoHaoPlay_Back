using Hao.Library;
using Hao.Utility;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Hao.Core.Extensions
{
    public class CurrentUser : ICurrentUser
    {
        private readonly HttpContext _httpContext;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }


        /// <summary>
        ///用户编号 
        /// </summary>
        public long Id
        {
            get => _httpContext == null ? -1 : HConvert.ToLong0(_httpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sid)?.Value);
        }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name
        {
            get => _httpContext == null ? "系统" : _httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimsName.Name)?.Value.ToString();
        }

        /// <summary>
        /// 用户角色等级
        /// </summary>
        public int RoleLevel
        {
            get => _httpContext == null ? 1 : HConvert.ToInt0(_httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimsName.RoleLevel)?.Value);
        }

        public string Jti 
        {
            get
            {
                if (_httpContext == null) return "";
                var jti = _httpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;
                return jti;
            }
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
