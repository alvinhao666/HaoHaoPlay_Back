using Hao.Utility;

namespace Hao.Enum
{
    /// <summary>
    /// 模块类型
    /// </summary>
    [H_EnumDescription("模块类型")]
    public enum ModuleType
    {
        /// <summary>
        /// 系统
        /// </summary>
        [H_EnumDescription("系统")]
        System,
        /// <summary>
        /// 主菜单
        /// </summary>
        [H_EnumDescription("主菜单")]
        Main,
        /// <summary>
        /// 子菜单
        /// </summary>
        [H_EnumDescription("子菜单")]
        Sub,
        /// <summary>
        /// 资源
        /// </summary>
        [H_EnumDescription("资源")]
        Resource
    }
}