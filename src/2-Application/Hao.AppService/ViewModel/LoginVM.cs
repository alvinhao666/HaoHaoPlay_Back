using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.AppService.ViewModel
{
    public class LoginVM
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
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 令牌
        /// </summary>
        public string Jwt { get; set; }
    }
}
