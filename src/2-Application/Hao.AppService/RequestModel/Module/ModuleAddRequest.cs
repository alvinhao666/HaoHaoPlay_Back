using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Hao.Enum;

namespace Hao.AppService
{
    /// <summary>
    /// 添加模块请求
    /// </summary>
    public class ModuleAddRequest
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 应用图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 路由地址
        /// </summary>
        public string RouterUrl { get; set; }

        /// <summary>
        /// 父级id
        /// </summary>
        public long? ParentId { get; set; }
        
        /// <summary>
        /// 模块类型
        /// </summary>
        public ModuleType? Type { get; set; }

        /// <summary>
        /// 排序值
        /// </summary>
        public int? Sort { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }
    }

    /// <summary>
    /// 验证
    /// </summary>
    public class ModuleAddValidator : AbstractValidator<ModuleAddRequest>
    {
        public ModuleAddValidator()
        {

            RuleFor(x => x.Name).MustHasValue("模块名称");

            RuleFor(x => x.Icon).MustHasValue("模块图标").When(a => a.Type == ModuleType.Main);

            RuleFor(x => x.RouterUrl).MustHasValue("子应用路由地址").When(a => a.Type == ModuleType.Sub);

            RuleFor(x => x.Alias).MustHasValue("别名").When(a => a.Type == ModuleType.Sub);

            RuleFor(x => x.Type).MustHasValue("模块类型");

            RuleFor(x => x.ParentId).MustHasValue("父节点Id");
        }
    }

}
