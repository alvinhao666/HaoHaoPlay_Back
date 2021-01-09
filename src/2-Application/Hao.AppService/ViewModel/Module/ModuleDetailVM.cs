using System.Collections.Generic;
using Hao.Enum;

namespace Hao.AppService
{
    public class ModuleDetailVM
    {
        public string Id { get; set; }

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
        public string ParentId { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public ModuleType? Type { get; set; }
        
        /// <summary>
        /// 排序值
        /// </summary>
        public int? Sort { get; set; }
        
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 资源项
        /// </summary>
        public List<ResourceItemVM> Resources { get; set; }
    }
}