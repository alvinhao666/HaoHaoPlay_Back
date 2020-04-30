using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.AppService
{
    /// <summary>
    /// 字典添加请求
    /// </summary>
    public class DictAddRequest
    {
        /// <summary>
        /// 字典名称
        /// </summary>
        public string DictName { get; set; }

        /// <summary>
        /// 字典编码
        /// </summary>
        public string DictCode { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 验证
    /// </summary>
    public class DictAddRequestValidator : AbstractValidator<DictAddRequest>
    {
        public DictAddRequestValidator()
        {
            RuleFor(x => x.DictName).NotEmpty().WithMessage("字典名称不能为空");

            RuleFor(x => x.DictCode).NotEmpty().WithMessage("字典编码不能为空");
        }
    }
}
