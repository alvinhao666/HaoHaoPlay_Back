using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.AppService
{
    /// <summary>
    /// 修改密码请求
    /// </summary>
    public class PwdUpdateInput
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string RePassword { get; set; }
    }

    /// <summary>
    /// 验证
    /// </summary>
    public class PwdUpdateValidator : AbstractValidator<PwdUpdateInput>
    {
        public PwdUpdateValidator()
        {
            RuleFor(x => x.OldPassword).MustHasValue("旧密码");

            RuleFor(x => x.NewPassword).MustFixedLength("新密码", 6, 16);

            RuleFor(x => x.NewPassword).Must((model, a) => model.OldPassword != a).WithMessage("新密码和旧密码不能相同");

            RuleFor(x => x.RePassword).MustHasValue("重复密码").Must((model, a) => model.NewPassword == a).WithMessage("两次输入密码不匹配");
        }
    }
}
