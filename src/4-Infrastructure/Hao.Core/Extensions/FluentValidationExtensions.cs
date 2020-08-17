using System;

namespace FluentValidation
{
    public static class FluentValidationExtensions
    {
        /// <summary>
        /// 判断提示不能为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, TProperty> MustHasValue<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, string fieldName)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{fieldName}不能为空");
        }

        /// <summary>
        /// 判断枚举不能为空且数据值正确
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, TProperty?> EnumMustHasValue<T, TProperty>(this IRuleBuilder<T, TProperty?> ruleBuilder, string enumName) where TProperty : struct
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{enumName}不能为空")
                .IsInEnum().WithMessage($"{enumName}数据值有误");
        }
    }
}
