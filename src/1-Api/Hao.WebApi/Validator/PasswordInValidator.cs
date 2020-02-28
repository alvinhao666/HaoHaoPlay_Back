using FluentValidation;
using Hao.AppService.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.WebApi
{
    public class PasswordInValidator : AbstractValidator<PasswordIn>
    {
        public PasswordInValidator()
        {
            RuleFor(x => x.OldPassword).NotEmpty().WithMessage("旧密码不能为空");

            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("新密码不能为空");

            RuleFor(x => x.RePassword).NotEmpty().WithMessage("重复密码不能为空").When(a => a.RePassword != a.NewPassword).WithMessage("两次输入密码不匹配");
        }
    }
}
