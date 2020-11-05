using Hao.Core;
using Hao.Enum;
using System;
using System.Collections.Generic;

namespace Hao.AppService
{
    /// <summary>
    /// 用户列表查询
    /// </summary>
    public class UserQueryInput : QueryInput
    {
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Gender? Gender { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

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
        public SortUser[] SortFields { get; set; }

        /// <summary>
        /// 排序类型
        /// </summary>
        public SortType[] SortTypes { get; set; }
    }

    /// <summary>
    /// 排序枚举
    /// </summary>
    public enum SortUser
    {
        /// <summary>
        /// 年龄
        /// </summary>
        Age
    }
}
