﻿using FluentValidation;

namespace Hao.AppService
{
    /// <summary>
    /// 添加资源请求
    /// </summary>
    public class ResourceAddRequest
    {
        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 父级id
        /// </summary>
        public long? ParentId { get; set; }
    }

    /// <summary>
    /// 验证
    /// </summary>
    public class ResourceAddValidator : AbstractValidator<ResourceAddRequest>
    {
        public ResourceAddValidator()
        {
            RuleFor(x => x.Name).MustHasValue("资源名称");

            RuleFor(x => x.ParentId).MustHasValue("父节点Id");
        }
    }
}