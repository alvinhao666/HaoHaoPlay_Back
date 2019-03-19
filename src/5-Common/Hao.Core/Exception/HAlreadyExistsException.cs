using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Core
{
    public class HAlreadyExistsException : HException
    {
        public HAlreadyExistsException() { }

        public HAlreadyExistsException(int code)
            : base(code) { }

        public HAlreadyExistsException(string message)
            : base(message) { }

        public HAlreadyExistsException(string message, int code)
            : base(message, code) { }

        public HAlreadyExistsException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
