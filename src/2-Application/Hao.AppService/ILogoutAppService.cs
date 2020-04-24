using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hao.AppService
{
    public interface ILogoutAppService
    {
        /// <summary>
        /// 注销当前登录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="jti"></param>
        /// <returns></returns>
        Task Logout(long userId, string jti);
    }
}
