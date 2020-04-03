using Hao.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Hao.Enum;

namespace Hao.Model
{
    /// <summary>
    /// 系统模块
    /// </summary>
    public class SysModule : FullAuditedEntity<long>
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 应用图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 路由地址
        /// </summary>
        public string RouterUrl { get; set; }

        /// <summary>
        /// 父级id
        /// </summary>
        public long? ParentId { get; set; }
        
        /// <summary>
        /// 模块类型
        /// </summary>
        public ModuleType? Type { get; set; }
        
        /// <summary>
        /// 排序值
        /// </summary>
        public int? Sort { get; set; }

        /// <summary>
        /// 层级（一层64个)
        /// </summary>
        public int? Layer { get; set; }

        /// <summary>
        /// 权限数字
        /// </summary>
        public string Number { get; set; }
    }
}
