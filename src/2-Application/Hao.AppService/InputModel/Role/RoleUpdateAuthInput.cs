using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.AppService
{
    /// <summary>
    /// 角色更新权限
    /// </summary>
    public class RoleUpdateAuthInput
    {
        /// <summary>
        /// 模块id
        /// </summary>
        public List<long> ModuleIds { get; set; }
    }
}
