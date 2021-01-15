using Hao.Core;

namespace Hao.Model
{
    public class SysRole : Entity<long>
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所拥有的权限
        /// </summary>
        public string AuthNumbers { get; set; }
        
        /// <summary>
        /// 角色等级
        /// </summary>
        public int? Level { get; set; }
    }
}