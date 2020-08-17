using FluentValidation;

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
            RuleFor(x => x.ItemName).MustHasValue("数据项名称");

            RuleFor(x => x.ItemValue).MustHasValue("数据项值");

            RuleFor(x => x.ParentId).MustHasValue("字典id");
        }
    }
}
