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
}
