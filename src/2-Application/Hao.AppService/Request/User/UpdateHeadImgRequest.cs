using FluentValidation;

namespace Hao.AppService
{
    public class UpdateHeadImgRequest
    {
        public string Base64Str { get; set; } 
    }

    public class UpdateHeadImgValidator : AbstractValidator<UpdateHeadImgRequest>
    {
        public UpdateHeadImgValidator()
        {
            RuleFor(x => x.Base64Str).NotEmpty().WithMessage("头像不能为空");
        }
    }
}