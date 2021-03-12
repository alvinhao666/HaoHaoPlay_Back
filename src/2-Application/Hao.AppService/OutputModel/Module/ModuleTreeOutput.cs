using Hao.Enum;
using System.Collections.Generic;

namespace Hao.AppService
{
    public class ModuleTreeOutput
    {
        public string key { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string title { get; set; }

        // /// <summary>
        // /// 应用图标
        // /// </summary>
        // public string Icon { get; set; }

        // /// <summary>
        // /// 路由地址
        // /// </summary>
        // public string RouterUrl { get; set; }

        // /// <summary>
        // /// 父级id
        // /// </summary>
        // public string ParentId { get; set; }

        /// <summary>
        /// 子模块
        /// </summary>
        public List<ModuleTreeOutput> children { get; set; }

        /// <summary>
        /// 是否展开
        /// </summary>
        public bool expanded { get; set; }

        /// <summary>
        /// 是否叶子节点
        /// </summary>
        public bool isLeaf { get; set; }
    }
}