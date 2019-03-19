using FluentValidation;
using Hao.AppService.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.WebApi
{
    public class UserVMInValidator : AbstractValidator<UserVMIn>
    {
        public UserVMInValidator()
        {
            RuleFor(x => x.Email)
                    .NotEmpty()
                    .WithMessage("FirstName is mandatory.");
        }
    }
}
