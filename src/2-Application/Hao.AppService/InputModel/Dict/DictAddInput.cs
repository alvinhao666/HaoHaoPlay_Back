using FluentValidation;

namespace Hao.AppService
{
    /// <summary>
    /// 字典添加请求
    /// </summary>
    public class DictAddInput
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
    public class DictAddRequestValidator : AbstractValidator<DictAddInput>
    {
        public DictAddRequestValidator()
        {
            RuleFor(x => x.DictName).MustHasValue("字典名称");

            RuleFor(x => x.DictCode).MustHasValue("字典编码");
        }
    }
}
