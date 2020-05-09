using FluentValidation;

namespace Hao.AppService
{
    /// <summary>
    /// 添加资源请求
    /// </summary>
    public class ResourceAddRequest
    {
        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 父级id
        /// </summary>
        public long? ParentId { get; set; }
    }

    /// <summary>
    /// 验证
    /// </summary>
    public class ResourceAddValidator : AbstractValidator<ResourceAddRequest>
    {
        public ResourceAddValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("资源名称不能为空");

            RuleFor(x => x.ParentId).NotEmpty().WithMessage("父节点Id不能为空");
        }
    }
}
