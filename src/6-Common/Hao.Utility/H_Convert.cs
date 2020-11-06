using System;
using System.IO;

namespace Hao.Utility
{
    public static class H_Convert
    {
        public static int? ToIntOrNull(this object value)
        {
            if (int.TryParse(value.ToSafeString(), out var result)) return result;

            return null;     
        }

        public static int ToInt(this object value)
        {
            return ToIntOrNull(value) ?? 0;
        }

        public static float? ToFloatOrNull(this object value)
        {
            if (float.TryParse(value.ToSafeString(), out var result)) return result;

            return null;
        }

        public static float ToFloat(this object value)
        {
            return ToFloatOrNull(value) ?? 0;
        }

        public static decimal? ToDecimalOrNull(this object value)
        {
            if (decimal.TryParse(value.ToSafeString(), out var result)) return result;

            return null;
        }

        public static decimal ToDecimal(this object value)
        {
            return ToDecimalOrNull(value) ?? 0;
        }

        public static double? ToDoubleOrNull(this object value)
        {
            if (double.TryParse(value.ToSafeString(), out var result)) return result;

            return null;
        }

        public static double ToDouble(this object value)
        {
            return ToDoubleOrNull(value) ?? 0;
        }

        public static long? ToLongOrNull(this object value)
        {
            if (long.TryParse(value.ToSafeString(), out var result)) return result;

            return null;
        }

        public static long ToLong(this object value)
        {
            return ToLongOrNull(value) ?? 0;
        }

        public static bool? ToBoolOrNull(this object value)
        {
            bool? outPut;
            switch (value.ToSafeString().ToLower())
            {
                case "0":
                    outPut=false;
                    break;
                case "否":
                    outPut = false;
                    break;
                case "不":
                    outPut = false;
                    break;
                case "no":
                    outPut = false;
                    break;
                case "fail":
                    outPut = false;
                    break;
                case "1":
                    outPut = true;
                    break;
                case "是":
                    outPut = true;
                    break;
                case "ok":
                    outPut = true;
                    break;
                case "yes":
                    outPut = true;
                    break;
                default:
                    outPut = null;
                    break;
            }

            if (outPut != null) return outPut.Value;

            return bool.TryParse(value.ToSafeString(), out var result) ? (bool?)result : null;
        }

        public static bool ToBool(this object value)
        {
            return ToBoolOrNull(value) ?? false;
        }

        public static Guid? ToGuidOrNull(this object value)
        {
            if (Guid.TryParse(value.ToSafeString(), out var result)) return result;

            return null;
        }

        public static Guid ToGuid(this object input)
        {
            return ToGuidOrNull(input) ?? Guid.Empty;
        }

        public static DateTime? ToDateTimeOrNull(this object value)
        {
            if (DateTime.TryParse(value.ToSafeString(), out var result)) return result;

            return null;
        }

        public static DateTime ToDate(this object value)
        {
            return ToDateTimeOrNull(value) ?? DateTime.MinValue;
        }


        /// <summary> 
        /// 将 Stream 转成 byte[] 
        /// </summary> 
        public static byte[] ToBytes(this Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

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
