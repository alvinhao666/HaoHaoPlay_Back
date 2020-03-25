using FluentValidation;
using Hao.AppService.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.WebApi
{
    public class ModuleAddValidator : AbstractValidator<ModuleAddRequest>
    {
        public ModuleAddValidator()
        {

            RuleFor(x => x.Name).NotEmpty().WithMessage("模块名称不能为空");

            RuleFor(x => x.Icon).NotEmpty().WithMessage("模块图标不能为空").When(a=> string.IsNullOrWhiteSpace(a.ParentId));

            RuleFor(x => x.RouterUrl).NotEmpty().WithMessage("子应用路由地址不能为空").When(a => !string.IsNullOrWhiteSpace(a.ParentId));
        }
    }
}
