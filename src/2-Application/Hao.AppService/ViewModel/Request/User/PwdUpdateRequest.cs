using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.AppService.ViewModel
{
    public class PwdUpdateRequest
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string RePassword { get; set; }
    }

    public class PwdUpdateValidator : AbstractValidator<PwdUpdateRequest>
    {
        public PwdUpdateValidator()
        {
            RuleFor(x => x.OldPassword).NotEmpty().WithMessage("旧密码不能为空");

            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("新密码不能为空").Length(6, 16).WithMessage("新密码长度应在6~16个字符");

            RuleFor(x => x.RePassword).NotEmpty().WithMessage("重复密码不能为空").When(a => a.RePassword != a.NewPassword).WithMessage("两次输入密码不匹配");
        }
    }
}
