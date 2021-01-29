using Hao.Utility;

namespace Hao.Enum
{
    /// <summary>
    /// 密码强弱等级
    /// </summary>
    [H_EnumDescription("密码强弱等级")]
    public enum PasswordLevel
    {
        /// <summary>
        /// 弱
        /// </summary>
        [H_EnumDescription("弱")]
        Weak,
        /// <summary>
        /// 中
        /// </summary>
        [H_EnumDescription("中")]
        Medium,
        /// <summary>
        /// 强
        /// </summary>
        [H_EnumDescription("强")]
        Strong
    }
}
