using FluentValidation;

namespace Hao.AppService
{
    public class DictItemUpdateInput
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
    public class DictItemUpdateRequestValidator : AbstractValidator<DictItemUpdateInput>
    {
        public DictItemUpdateRequestValidator()
        {
            RuleFor(x => x.ItemName).MustHasValue("数据项名称");

            RuleFor(x => x.ItemValue).MustHasValue("数据项值");

        }
    }
}