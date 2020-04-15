using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Hao.Enum;

namespace Hao.AppService
{
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
    }


    public class ModuleAddValidator : AbstractValidator<ModuleAddRequest>
    {
        public ModuleAddValidator()
        {

            RuleFor(x => x.Name).NotEmpty().WithMessage("模块名称不能为空");

            RuleFor(x => x.Icon).NotEmpty().WithMessage("模块图标不能为空").When(a => a.Type == ModuleType.Main);

            RuleFor(x => x.RouterUrl).NotEmpty().WithMessage("子应用路由地址不能为空").When(a => a.Type == ModuleType.Sub);

            RuleFor(x => x.Type).NotEmpty().WithMessage("模块类型不能为空");

            RuleFor(x => x.ParentId).NotEmpty().WithMessage("父节点Id不能为空");
        }
    }

}
