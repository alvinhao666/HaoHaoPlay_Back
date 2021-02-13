using System;
using System.Dynamic;

namespace Hao.Utility
{
    public class H_Type
    {
        public static readonly Type IntType = typeof(int);
        public static readonly Type IntTypeNull = typeof(int?);
        public static readonly Type LongType = typeof(long);
        public static readonly Type LongTypeNull = typeof(long?);
        public static readonly Type GuidType = typeof(Guid);
        public static readonly Type GuidTypeNull = typeof(Guid?);
        public static readonly Type BoolType = typeof(bool);
        public static readonly Type BoolTypeNull = typeof(bool?);
        public static readonly Type ByteType = typeof(Byte);
        public static readonly Type ObjType = typeof(object);
        public static readonly Type DobType = typeof(double);
        public static readonly Type DobTypeNull = typeof(double?);
        public static readonly Type FloatType = typeof(float);
        public static readonly Type FloatTypeNull = typeof(float?);
        public static readonly Type ShortType = typeof(short);
        public static readonly Type ShortTypeNull = typeof(short?);
        public static readonly Type DecType = typeof(decimal);
        public static readonly Type DecTypeNull = typeof(decimal?);
        public static readonly Type StringType = typeof(string);
        public static readonly Type DateType = typeof(DateTime);
        public static readonly Type DateTypeNull = typeof(DateTime?);
        public static readonly Type DateTimeOffsetType = typeof(DateTimeOffset);
        public static readonly Type TimeSpanType = typeof(TimeSpan);
        public static readonly Type ByteArrayType = typeof(byte[]);
        public static readonly Type DynamicType = typeof(ExpandoObject);
    }
}