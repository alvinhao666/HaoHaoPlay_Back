using Hao.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.AppService.ViewModel
{
    public class UserAddRequest
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int?  Age { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public Gender? Gender { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 微信
        /// </summary>
        public string WeChat { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 个人简介
        /// </summary>
        public string Profile { get; set; }

        /// <summary>
        /// 家庭地址
        /// </summary>
        public string HomeAddress { get; set; }
    }
}
