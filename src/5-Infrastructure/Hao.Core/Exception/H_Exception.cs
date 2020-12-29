using System;

namespace Hao.Core
{
    public class H_Exception : Exception
    {
        public int? Code { get; private set; }

        public H_Exception() { }

        public H_Exception(int? code)
        {
            this.Code = Code;
        }

        public H_Exception(string message) : base(message) { }

        public H_Exception(string message, int? code) : base(message)
        {
            this.Code = code;
        }

        public H_Exception(string message, Exception innerException) : base(message, innerException) { }
    }


    public static class H_Assert
    {
        public static void IsTrue<T>(bool condition, string msg) where T : Exception, new()
        {
            if (condition)
            {
                var ex = Activator.CreateInstance(typeof(T), new object[] { msg }) as T;
                throw ex;
            }
        }

        public static void IsTrue<T>(bool condition, string msg, int code) where T : Exception, new()
        {
            if (condition)
            {
                var ex = Activator.CreateInstance(typeof(T), new object[] { msg, code }) as T;
                throw ex;
            }
        }

        public static void IsFalse<T>(bool condition, string msg) where T : Exception, new()
        {
            if (!condition)
            {
                var ex = Activator.CreateInstance(typeof(T), new object[] { msg }) as T;
                throw ex;
            }
        }

        public static void IsFalse<T>(bool condition, string msg, int code) where T : Exception, new()
        {
            if (!condition)
            {
                var ex = Activator.CreateInstance(typeof(T), new object[] { msg, code }) as T;
                throw ex;
            }
        }
    }
}
