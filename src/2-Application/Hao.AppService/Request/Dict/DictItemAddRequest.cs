using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.AppService
{
    /// <summary>
    /// 字典数据项添加请求
    /// </summary>
    public class DictItemAddRequest
    {
        /// <summary>
        /// 数据项名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 数据项值
        /// </summary>
        public int? ItemValue { get; set; }

        /// <summary>
        /// 父级 字典id
        /// </summary>
        public long? ParentId { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remark { get; set; }
        
        /// <summary>
        /// 排序值
        /// </summary>
        public int? Sort { get; set; }
    }

    /// <summary>
    /// 验证
    /// </summary>
    public class DictItemAddRequestValidator : AbstractValidator<DictItemAddRequest>
    {
        public DictItemAddRequestValidator()
        {
            RuleFor(x => x.ItemName).NotEmpty().WithMessage("数据项名称不能为空");

            RuleFor(x => x.ItemValue).NotEmpty().WithMessage("数据项值不能为空");

            RuleFor(x => x.ParentId).NotEmpty().WithMessage("字典id不能为空");
        }
    }
}
