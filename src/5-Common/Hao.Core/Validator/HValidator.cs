using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hao.Core
{
    /// <summary>
    /// 验证
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HValidator<T> : AbstractValidator<T>
    {
        /// <summary>
        /// 参数验证失败代码
        /// </summary>
        private static int ErrorCode { get; set; } = -3;

        /// <summary>
        /// 重写参数验证基类（不通过抛异常）
        /// </summary>
        public override ValidationResult Validate(ValidationContext<T> context)
        {
            var result = base.Validate(context);

            if (!result.IsValid && result.Errors != null && result.Errors.Count > 0)
            {
                var error = result.Errors.FirstOrDefault();
                int code;
                if (!int.TryParse(error.ErrorCode, out code))
                {
                    code = ErrorCode;
                }
                throw new HException(error.ErrorMessage, code);
            }

            return result;
        }
    }
}
