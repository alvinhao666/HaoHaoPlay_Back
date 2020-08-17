using FluentValidation;

namespace Hao.AppService
{
    /// <summary>
    /// �޸�ͷ������
    /// </summary>
    public class UpdateHeadImgRequest
    {
        public string Base64Str { get; set; } 
    }

    /// <summary>
    /// ��֤
    /// </summary>
    public class UpdateHeadImgValidator : AbstractValidator<UpdateHeadImgRequest>
    {
        public UpdateHeadImgValidator()
        {
            RuleFor(x => x.Base64Str).MustHasValue("ͷ��");
        }
    }
}