using FluentValidation;

namespace Hao.AppService
{
    /// <summary>
    /// 修改字典
    /// </summary>
    public class DictUpdateRequest
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
    public class DictUpdateRequestValidator : AbstractValidator<DictUpdateRequest>
    {
        public DictUpdateRequestValidator()
        {
            RuleFor(x => x.DictName).NotEmpty().WithMessage("字典名称不能为空");

            RuleFor(x => x.DictCode).NotEmpty().WithMessage("字典编码不能为空");
        }
    }
}