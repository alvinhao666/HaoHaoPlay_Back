using FluentValidation;
using Hao.Utility;

namespace Hao.AppService
{
    /// <summary>
    /// 添加资源请求
    /// </summary>
    public class ResourceAddInput
    {
        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 父级id
        /// </summary>
        public long? ParentId { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        public string Alias { get; set; }
    }

    /// <summary>
    /// 验证
    /// </summary>
    public class ResourceAddValidator : AbstractValidator<ResourceAddInput>
    {
        public ResourceAddValidator()
        {
            RuleFor(x => x.Name).MustHasValue("资源名称");

            RuleFor(x => x.ParentId).MustHasValue("父节点Id");

            RuleFor(x => x.Alias).MustHasValue("别名").Must(a => H_Validator.IsLetter(a)).WithMessage("别名只能输入英文");
        }
    }
}
