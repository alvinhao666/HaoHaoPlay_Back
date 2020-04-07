using FluentValidation;
using Hao.AppService.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.WebApi
{
    public class ResourceUpdateValidator : AbstractValidator<ResourceUpdateRequest>
    {
        public ResourceUpdateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("资源名称不能为空");

            RuleFor(x => x.Sort).NotEmpty().WithMessage("排序值不能为空");
        }
    }
}
