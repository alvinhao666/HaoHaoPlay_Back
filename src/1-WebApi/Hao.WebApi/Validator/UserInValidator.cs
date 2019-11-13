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
            RuleFor(x => x.Gender).NotEmpty().WithMessage("性别不能为空").IsInEnum().WithMessage("性别数据有误");
        }
    }
}
