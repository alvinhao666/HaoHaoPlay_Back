using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.AppService.ViewModel
{
    public class LoginOut
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 令牌
        /// </summary>
        public string JwtToken { get; set; }
    }
}
