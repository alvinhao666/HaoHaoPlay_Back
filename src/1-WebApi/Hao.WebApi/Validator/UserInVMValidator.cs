using FluentValidation;
using Hao.AppService.ViewModel;
using Hao.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.WebApi
{
    public class UserVMInValidator : AbstractValidator<UserIn>
    {
        public UserVMInValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("邮箱不能为空");
        }
    }
}
