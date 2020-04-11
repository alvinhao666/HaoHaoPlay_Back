using System.Collections.Generic;
using Hao.Core;

namespace Hao.Model
{
    public class SysRole : FullAuditedEntity<long>
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所拥有的权限
        /// </summary>
        public List<long> AuthNumber { get; set; }
    }
}