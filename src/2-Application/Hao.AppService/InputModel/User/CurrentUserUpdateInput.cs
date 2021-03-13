using FluentValidation;
using Hao.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using Hao.Utility;

namespace Hao.AppService
{
    /// <summary>
    /// 更新当前用户信息请求
    /// </summary>
    public class CurrentUserUpdateInput
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 微信
        /// </summary>
        public string WeChat { get; set; }

        // /// <summary>
        // /// 昵称
        // /// </summary>
        // public string NickName { get; set; }

        /// <summary>
        /// 个人简介
        /// </summary>
        public string Profile { get; set; }

        /// <summary>
        /// 家庭地址
        /// </summary>
        public string HomeAddress { get; set; }
    }


    /// <summary>
    /// 验证
    /// </summary>
    public class CurrentUserUpdateValidator : AbstractValidator<CurrentUserUpdateInput>
    {
        public CurrentUserUpdateValidator()
        {
            RuleFor(x => x.Phone).MustHasValue("手机").Must(x => H_Validator.IsMobile(x)).WithMessage("手机号格式有误");

            // RuleFor(x => x.Gender).EnumMustHasValue("性别");
        }
    }
}
