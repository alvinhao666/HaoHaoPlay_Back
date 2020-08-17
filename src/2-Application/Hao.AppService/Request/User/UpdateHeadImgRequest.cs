using FluentValidation;

namespace Hao.AppService
{
    /// <summary>
    /// 修改头像请求
    /// </summary>
    public class UpdateHeadImgRequest
    {
        public string Base64Str { get; set; } 
    }

    /// <summary>
    /// 验证
    /// </summary>
    public class UpdateHeadImgValidator : AbstractValidator<UpdateHeadImgRequest>
    {
        public UpdateHeadImgValidator()
        {
            RuleFor(x => x.Base64Str).MustHasValue("头像");
        }
    }
}