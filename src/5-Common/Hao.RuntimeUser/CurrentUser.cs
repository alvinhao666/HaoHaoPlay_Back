﻿using Microsoft.AspNetCore.Http;

namespace Hao.RuntimeUser
{
    public class CurrentUser : ICurrentUser
    {
        private IHttpContextAccessor _httpContextAccessor;

        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        ///用户编号 
        /// </summary>
        public string UserId
        {
            get => HConvert.ToLong((_session.GetString("CurrentUser_UserId")));
            set => _session.SetString("CurrentUser_UserId", value.ToString());
        }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName
        {
            get => _session.GetString("CurrentUser_UserName");
            set => _session.SetString("CurrentUser_UserName", value);
        }

    }
}