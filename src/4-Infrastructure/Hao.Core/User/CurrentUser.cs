using Hao.Utility;
using Microsoft.AspNetCore.Http;

namespace Hao.Core
{
    public class CurrentUser : ICurrentUser
    {
        private IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        ///用户编号 
        /// </summary>
        public long? Id
        {
            get => _httpContextAccessor.HttpContext == null ? -1 : HConvert.ToLong(_httpContextAccessor.HttpContext.Session.GetString("CurrentUser_UserId"));
            set => _httpContextAccessor.HttpContext.Session.SetString("CurrentUser_UserId", value.ToString());
        }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name
        {
            get => _httpContextAccessor.HttpContext == null ? "系统" : _httpContextAccessor.HttpContext.Session.GetString("CurrentUser_UserName");
            set => _httpContextAccessor.HttpContext.Session.SetString("CurrentUser_UserName", value);
        }

    }
}
