using System;

namespace Hao.Utility
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取描述
        /// </summary>
        /// <param name="enum">枚举对象</param>
        /// <returns></returns>
        public static string ToDescription(this Enum @enum)
        {
            if (@enum == null) return null;
            var enumType = @enum.GetType();
            if (!Enum.IsDefined(enumType, @enum)) return null;
            var fieldName = @enum.ToString();
            var attribute = H_Description.Get(enumType, fieldName);
            return attribute?.Description;
        }
    }
}
