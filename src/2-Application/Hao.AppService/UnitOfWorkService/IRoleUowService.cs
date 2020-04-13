using Hao.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hao.AppService
{
    public interface IRoleUowService
    {
        /// <summary>
        /// 更新权限
        /// </summary>
        /// <param name="role"></param>
        void UpdateAuth(SysRole role);
    }
}
