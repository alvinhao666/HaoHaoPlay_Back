using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.AppService
{
    /// <summary>
    /// 更新资源请求
    /// </summary>
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

    /// <summary>
    /// 验证
    /// </summary>
    public class ResourceUpdateValidator : AbstractValidator<ResourceUpdateRequest>
    {
        public ResourceUpdateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("资源名称不能为空");

            RuleFor(x => x.Sort).NotEmpty().WithMessage("排序值不能为空");
        }
    }
}
