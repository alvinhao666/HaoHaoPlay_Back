using Hao.Utility;

namespace Hao.Enum
{
    /// <summary>
    /// 角色等级
    /// </summary>
    [H_EnumDescription("角色等级")]
    public enum RoleLevelType
    {
        /// <summary>
        /// 超级管理员
        /// </summary>
        [H_EnumDescription("超级管理员")]
        SuperAdministrator,

        /// <summary>
        /// 管理员
        /// </summary>
        [H_EnumDescription("管理员")]
        Administrator,

        /// <summary>
        /// 普通用户
        /// </summary>
        [H_EnumDescription("普通用户")]
        User
    }
}
