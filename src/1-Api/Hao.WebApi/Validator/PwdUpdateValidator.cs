using FluentValidation;
using Hao.AppService.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.WebApi
{
    public class PwdUpdateValidator : AbstractValidator<PwdUpdateRequest>
    {
        public PwdUpdateValidator()
        {
            RuleFor(x => x.OldPassword).NotEmpty().WithMessage("旧密码不能为空");

            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("新密码不能为空").WithMessage("新密码长度应在6~16个字符");

            RuleFor(x => x.RePassword).NotEmpty().WithMessage("重复密码不能为空").When(a => a.RePassword != a.NewPassword).WithMessage("两次输入密码不匹配");
        }
    }
}
