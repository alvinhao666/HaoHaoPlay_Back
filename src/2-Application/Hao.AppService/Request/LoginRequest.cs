using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.AppService
{
    /// <summary>
    /// 登录请求
    /// </summary>
    public class LoginRequest
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
        /// 是否选择十天免登录
        /// </summary>
        public bool IsRememberLogin { get; set; }
    }

    /// <summary>
    /// 验证
    /// </summary>
    public class LoginValidator : AbstractValidator<LoginRequest>
    {
        public LoginValidator()
        {
            RuleFor(x => x.LoginName).NotEmpty().WithMessage("账号不能为空");

            RuleFor(x => x.Password).NotEmpty().WithMessage("密码不能为空");
        }
    }
}
