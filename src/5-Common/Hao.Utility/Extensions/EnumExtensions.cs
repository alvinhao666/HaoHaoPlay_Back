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
        public static string GetDescription(this Enum @enum)
        {
            if (@enum == null) return null;
            var fieldName = @enum.ToString();
            var enumType = @enum.GetType();
            if (!Enum.IsDefined(enumType, @enum)) return null;
            var hDescriptionAttribute = H_Description.Get(enumType, fieldName);
            return hDescriptionAttribute?.Description;
        }

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="member">成员名或值,范例:Enum1枚举有成员A=0,则传入"A"或"0"获取 Enum1.A</param>
        public static TEnum Parse<TEnum>(object member)
        {
            string value = member.ToSafeString();
            if (string.IsNullOrWhiteSpace(value))
            {
                if (typeof(TEnum).IsGenericType)
                    return default(TEnum);
                throw new ArgumentNullException(nameof(member));
            }
            return (TEnum)Enum.Parse(H_Common.GetType<TEnum>(), value, true);
        }
    }
}
