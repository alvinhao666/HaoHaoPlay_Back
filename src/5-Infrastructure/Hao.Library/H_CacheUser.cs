using System;
using System.Collections.Generic;

namespace Hao.Library
{
    /// <summary>
    /// 缓存至redis的用户模型
    /// </summary>
    public class H_CacheUser
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 角色等级
        /// </summary>
        public int RoleLevel { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public List<long> AuthNums { get; set; }

        /// <summary>
        /// token值
        /// </summary>
        public string Jwt { get; set; }

        /// <summary>
        /// token唯一标识
        /// </summary>
        public string Jti { get; set; }

        /// <summary>
        /// 登录状态
        /// </summary>
        public LoginStatus LoginStatus { get; set; }

        /// <summary>
        /// 是否权限更新
        /// </summary>
        public bool IsAuthUpdate { get; set; }

        /// <summary>
        /// ip地址
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }
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
