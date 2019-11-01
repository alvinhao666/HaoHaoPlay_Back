using System;

namespace Hao.Core
{
    public class HException : Exception
    {
        public int Code { get; private set; }

        public HException() { }

        public HException(int code)
        {
            this.Code = Code;
        }

        public HException(string message)
            : base(message) { }

        public HException(string message, int code)
            : base(message)
        {
            this.Code = code;
        }

        public HException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
