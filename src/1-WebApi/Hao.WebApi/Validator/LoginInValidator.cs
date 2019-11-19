using FluentValidation;
using Hao.AppService.ViewModel;
using Hao.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.WebApi
{
    public class LoginInValidator : AbstractValidator<LoginIn>
    {
        public LoginInValidator()
        {
            RuleFor(x => x.LoginName).NotEmpty().WithMessage("账号不能为空");

            RuleFor(x => x.Password).NotEmpty().WithMessage("密码不能为空");
        }
    }
}
