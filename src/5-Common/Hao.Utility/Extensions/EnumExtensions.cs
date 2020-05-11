using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Utility
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取描述
        /// </summary>
        /// <param name="enum">枚举对象</param>
        /// <returns></returns>
        public static string GetDescription(this Enum @enum)
        {
            if (@enum == null) return null;
            var fieldName = @enum.ToString();
            var enumType = @enum.GetType();
            if (!Enum.IsDefined(enumType, @enum)) return null;
            var hDescriptionAttribute = H_Description.Get(enumType, fieldName);
            return hDescriptionAttribute?.Description;
        }
    }
}
