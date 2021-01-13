using Hao.Core;
using Hao.Enum;
using System;

namespace Hao.AppService
{
    /// <summary>
    /// 用户列表查询
    /// </summary>
    public class UserQueryInput : QueryInput
    {
        /// <summary>
        /// 姓名 模糊查询
        /// </summary>
        public string LikeName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Gender? Gender { get; set; }

        /// <summary>
        /// 手机号 模糊查询
        /// </summary>
        public string LikePhone { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? Enabled { get; set; }

        /// <summary>
        /// 最后登录开始时间
        /// </summary>
        public DateTime? LastLoginTimeStart { get; set; }

        /// <summary>
        /// 组后登录结束时间
        /// </summary>
        public DateTime? LastLoginTimeEnd { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public SortUser?[] SortFields { get; set; }
    }

    /// <summary>
    /// 排序枚举
    /// </summary>
    public enum SortUser
    {
        /// <summary>
        /// 年龄
        /// </summary>
        Age,

        /// <summary>
        /// 最后登录时间
        /// </summary>
        LastLoginTime
    }
}
