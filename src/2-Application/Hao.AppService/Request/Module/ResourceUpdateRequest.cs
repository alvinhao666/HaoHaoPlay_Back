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
        /// 排序值
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
            RuleFor(x => x.Name).MustHasValue("资源名称");

            RuleFor(x => x.Sort).MustHasValue("排序值");
        }
    }
}
