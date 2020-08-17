using FluentValidation;

namespace Hao.AppService
{
    /// <summary>
    /// 登录请求  abp框架 用户输入参数的验证工作也应该在应用层实现
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 是否选择三天免登录
        /// </summary>
        public bool IsRememberLogin { get; set; }

        /// <summary>
        /// 登录ip
        /// </summary>
        public string Ip { get; set; }
    }

    /// <summary>
    /// 验证
    /// </summary>
    public class LoginValidator : AbstractValidator<LoginRequest>
    {
        public LoginValidator()
        {
            RuleFor(x => x.LoginName).MustHasValue("账号");

            RuleFor(x => x.Password).MustHasValue("密码");
        }
    }
}
