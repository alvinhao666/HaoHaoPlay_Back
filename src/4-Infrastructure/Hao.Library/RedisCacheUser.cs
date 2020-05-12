using System.Collections.Generic;

namespace Hao.Library
{
    /// <summary>
    /// 用户认证实体
    /// </summary>
    public class RedisCacheUser
    {
        public long? Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public List<long> AuthNumbers { get; set; }

        /// <summary>
        /// token值
        /// </summary>
        public string Jwt { get; set; }

        /// <summary>
        /// 登录状态
        /// </summary>
        public LoginStatus? LoginStatus { get; set; }

        /// <summary>
        /// 是否权限更新
        /// </summary>
        public bool? IsAuthUpdate { get; set; }
    }

    /// <summary>
    /// 登录状态
    /// </summary>
    public enum LoginStatus
    {
        /// <summary>
        /// 未登录
        /// </summary>
        Offline,
        /// <summary>
        /// 登录
        /// </summary>
        Online
    }
}
