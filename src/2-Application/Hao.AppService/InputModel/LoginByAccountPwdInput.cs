using FluentValidation;

namespace Hao.AppService
{
    /// <summary>
    /// 登录请求  abp框架 用户输入参数的验证工作也应该在应用层实现
    /// </summary>
    public class LoginByAccountPwdInput
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 是否选择三天免登录
        /// </summary>
        public bool IsRememberLogin { get; set; }


        //public List<int> Codes { get; set; }
    }

    /// <summary>
    /// 验证
    /// </summary>
    public class LoginByAccountPwdValidator : AbstractValidator<LoginByAccountPwdInput>
    {
        public LoginByAccountPwdValidator()
        {
            RuleFor(x => x.Account).MustHasValue("账号");

            RuleFor(x => x.Password).MustHasValue("密码");

            //RuleFor(x => x.Codes).NotEmpty().WithMessage("Codes不能为空"); //Codes 为null和空集合 报错
        }
    }
}
