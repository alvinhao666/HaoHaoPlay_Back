using FluentValidation;
using Hao.Utility;

namespace Hao.AppService
{
    /// <summary>
    /// 更新资源请求
    /// </summary>
    public class ResourceUpdateInput
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        public string Name { get; set; }

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
    public class ResourceUpdateValidator : AbstractValidator<ResourceUpdateInput>
    {
        public ResourceUpdateValidator()
        {
            RuleFor(x => x.Name).MustHasValue("资源名称");

            RuleFor(x => x.Sort).MustHasValue("排序值");

            RuleFor(x => x.Alias).MustHasValue("别名").Must(a => H_Validator.IsLetter(a)).WithMessage("别名只能输入英文");
        }
    }
}
