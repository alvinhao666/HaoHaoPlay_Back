using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hao.AppService
{
    public interface ILogoutAppService
    {
        /// <summary>
        /// 注销登录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task Logout(long userId);
    }
}
