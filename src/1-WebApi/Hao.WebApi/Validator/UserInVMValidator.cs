using FluentValidation;
using Hao.AppService.ViewModel;
using Hao.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.WebApi
{
    public class UserVMInValidator : HValidator<UserVMIn>
    {
        public UserVMInValidator()
        {
            RuleFor(x => x.Email)
                    .NotEmpty()
                    .WithMessage("FirstName is mandatory.");
        }
    }
}
