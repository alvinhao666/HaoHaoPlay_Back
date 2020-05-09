using FluentValidation;
using Hao.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.AppService
{
    /// <summary>
    /// 修改用户请求
    /// </summary>
    public class UserUpdateRequest
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int? Age { get; set; }
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
        /// QQ
        /// </summary>
        public string QQ { get; set; }
    }

    /// <summary>
    /// 验证
    /// </summary>
    public class UserUpdateRequestValidator : AbstractValidator<UserUpdateRequest>
    {
        public UserUpdateRequestValidator()
        {

            RuleFor(x => x.Name).NotEmpty().WithMessage("姓名不能为空");

            RuleFor(x => x.Gender).NotEmpty().WithMessage("性别不能为空").IsInEnum().WithMessage("性别数据有误");

            RuleFor(x => x.Age).NotEmpty().WithMessage("年龄不能为空");
        }
    }
}
