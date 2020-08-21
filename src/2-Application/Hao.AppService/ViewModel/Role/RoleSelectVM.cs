using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.AppService
{
    /// <summary>
    /// 角色下拉框数据
    /// </summary>
    public class RoleSelectVM
    {
        /// <summary>
        /// 角色id
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }
    }
}
