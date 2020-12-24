using System;
using Hao.Core;
using Hao.Enum;

namespace Hao.Model
{
    /// <summary>
    /// 系统用户 
    /// </summary>
    public class SysUser : Entity<long>
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
        /// 真实姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int? Age { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Gender? Gender { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }
        /// <summary>
        /// 微信
        /// </summary>
        public string WeChat { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? Enabled { get; set; } 
        /// <summary>
        /// 名字首字母
        /// </summary>
        public string FirstNameSpell { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 最后登录IP
        /// </summary>
        public string LastLoginIP { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>
        public string HeadImgUrl { get; set; }

        /// <summary>
        /// 个人简介
        /// </summary>
        public string Profile { get; set; }

        /// <summary>
        /// 家庭地址
        /// </summary>
        public string HomeAddress { get; set; }

        /// <summary>
        /// 密码强度
        /// </summary>
        public PasswordLevel? PasswordLevel { get; set; }

        /// <summary>
        /// 角色id
        /// </summary>
        public long? RoleId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 拥有的权限数字
        /// </summary>
        public string AuthNumbers { get; set; }

        /// <summary>
        /// 角色等级
        /// </summary>
        public int? RoleLevel { get; set; }
    }
}