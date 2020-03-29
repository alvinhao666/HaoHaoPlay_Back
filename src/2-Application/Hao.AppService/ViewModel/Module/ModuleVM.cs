using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.AppService.ViewModel
{
    public class ModuleVM
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
        public List<ModuleVM> children { get; set; }

        // public bool expanded => children.Count > 0;
        
        /// <summary>
        /// 是否叶子节点
        /// </summary>
        public bool isLeaf { get; set; }
    }

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
    }
}