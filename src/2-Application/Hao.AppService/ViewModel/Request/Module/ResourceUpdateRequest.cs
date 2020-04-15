using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.AppService.ViewModel
{
    public class ResourceUpdateRequest
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort { get; set; }
    }

    public class ResourceUpdateValidator : AbstractValidator<ResourceUpdateRequest>
    {
        public ResourceUpdateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("资源名称不能为空");

            RuleFor(x => x.Sort).NotEmpty().WithMessage("排序值不能为空");
        }
    }
}
