using System;
using System.IO;

namespace Hao.Utility
{
    public static class H_Convert
    {
        public static int? ToIntOrNull(this object value)
        {
            if (int.TryParse(value.SafeString(), out var result)) return result;

            return null;     
        }

        public static int ToInt(this object value)
        {
            return ToIntOrNull(value) ?? 0;
        }

        public static float? ToFloatOrNull(this object value)
        {
            if (float.TryParse(value.SafeString(), out var result)) return result;

            return null;
        }

        public static float ToFloat(this object value)
        {
            return ToFloatOrNull(value) ?? 0;
        }

        public static decimal? ToDecimalOrNull(this object value)
        {
            if (decimal.TryParse(value.SafeString(), out var result)) return result;

            return null;
        }

        public static decimal ToDecimal(this object value)
        {
            return ToDecimalOrNull(value) ?? 0;
        }

        public static double? ToDoubleOrNull(this object value)
        {
            if (double.TryParse(value.SafeString(), out var result)) return result;

            return null;
        }

        public static double ToDouble(this object value)
        {
            return ToDoubleOrNull(value) ?? 0;
        }

        public static long? ToLongOrNull(this object value)
        {
            if (long.TryParse(value.SafeString(), out var result)) return result;

            return null;
        }

        public static long ToLong(this object value)
        {
            return ToLongOrNull(value) ?? 0;
        }

        public static bool? ToBoolOrNull(this object value)
        {
            bool? outPut = GetBool(value);

            if (outPut != null) return outPut.Value;

            return bool.TryParse(value.SafeString(), out var result) ? (bool?)result : null;
        }

        public static bool ToBool(this object value)
        {
            return ToBoolOrNull(value) ?? false;
        }

        public static Guid? ToGuidOrNull(this object value)
        {
            if (Guid.TryParse(value.SafeString(), out var result)) return result;

            return null;
        }

        public static Guid ToGuid(this object input)
        {
            return ToGuidOrNull(input) ?? Guid.Empty;
        }

        public static DateTime? ToDateTimeOrNull(this object value)
        {
            if (DateTime.TryParse(value.SafeString(), out var result)) return result;

            return null;
        }

        public static DateTime ToDate(this object value)
        {
            return ToDateTimeOrNull(value) ?? DateTime.MinValue;
        }


        /// <summary>
        /// 获取布尔值
        /// </summary>
        private static bool? GetBool(object input)
        {
            if (input == DBNull.Value) return null;

            switch (input.SafeString().ToLower())
            {
                case "0":
                    return false;
                case "否":
                    return false;
                case "不":
                    return false;
                case "no":
                    return false;
                case "fail":
                    return false;
                case "1":
                    return true;
                case "是":
                    return true;
                case "ok":
                    return true;
                case "yes":
                    return true;
                default:
                    return null;
            }
        }

        /// <summary> 
        /// 将 Stream 转成 byte[] 
        /// </summary> 
        public static byte[] ToBytes(this Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// <summary> 
        /// 将 byte[] 转成 Stream 
        /// </summary> 
        public static Stream ToStream(this byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
    }
}
