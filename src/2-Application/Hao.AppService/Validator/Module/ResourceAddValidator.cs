using FluentValidation;
using Hao.AppService.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.AppService
{
    public class ResourceAddValidator : AbstractValidator<ResourceAddRequest>
    {
        public ResourceAddValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("资源名称不能为空");

            RuleFor(x => x.ParentId).NotEmpty().WithMessage("父节点Id不能为空");
        }
    }
}
