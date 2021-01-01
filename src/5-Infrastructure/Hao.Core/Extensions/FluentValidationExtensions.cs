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
        public static IRuleBuilderOptions<T, TProperty> MustHasValue<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder, string fieldName)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{fieldName}不能为空");
        }

        /// <summary>
        /// 判断提示不能为空，且指定字符串长度范围
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <param name="fieldName"></param>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string> MustFixedLength<T>(this IRuleBuilder<T, string> ruleBuilder,
            string fieldName, int minLength, int maxLength)
        {
            return ruleBuilder
                .Length(minLength, maxLength)  //null和空字符串满足 minlength=0  先执行jsonconvert 去除模型空字符串 再执行验证
                .WithMessage($"{fieldName}的长度必须在{minLength}~{maxLength}个字符");
        }

        /// <summary>
        /// 判断枚举不能为空且数据值正确
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, TProperty?> EnumMustHasValue<T, TProperty>(
            this IRuleBuilder<T, TProperty?> ruleBuilder, string enumName) where TProperty : struct, Enum
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{enumName}不能为空")
                .IsInEnum().WithMessage($"{enumName}值有误");
        }
    }
}