using FluentValidation;
using Hao.AppService.ViewModel;
using Hao.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.WebApi
{
    public class UserInValidator : AbstractValidator<UserIn>
    {
        public UserInValidator()
        {

            RuleFor(x => x.Name).NotEmpty().WithMessage("姓名不能为空");

            RuleFor(x => x.Password).NotEmpty().WithMessage("密码不能为空").Length(6, 16).WithMessage("密码长度应在6~16个字符");

            RuleFor(x => x.Gender).NotEmpty().WithMessage("性别不能为空").IsInEnum().WithMessage("性别数据有误");
        }
    }
}
