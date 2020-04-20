using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.AppService.ViewModel
{
    public class RoleModuleVM
    {
        public List<RoleModuleItemVM> Nodes { get; set; }

        public List<string> CheckedKeys { get; set; }
    }

    public class RoleModuleItemVM
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
        public List<RoleModuleItemVM> children { get; set; }

        /// <summary>
        /// 展开
        /// </summary>
        public bool expanded { get; set; }
 
        /// <summary>
        /// 是否叶子节点
        /// </summary>
        public bool isLeaf { get; set; }

        ///// <summary>
        ///// 设置节点本身是否选中
        ///// </summary>
        //public bool @checked { get; set; }
    }
}
