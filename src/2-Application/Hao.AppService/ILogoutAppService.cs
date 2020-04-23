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
        /// <param name="jwt"></param>
        /// <returns></returns>
        Task Logout(long userId, string jti);

        /// <summary>
        /// 更新权限后,该账户所有登录注销
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task LogoutByUpdateAuth(long userId);
    }
}
