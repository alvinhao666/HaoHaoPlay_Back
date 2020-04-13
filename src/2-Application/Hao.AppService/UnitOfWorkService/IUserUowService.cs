using Hao.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hao.AppService
{
    public interface IUserUowService
    {
        /// <summary>
        /// 更新登录信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        void UpdateLogin(SysUser user);
    }
}
