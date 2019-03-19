using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Core
{
    public class HNotFoundException:HException
    {
        public HNotFoundException() { }

        public HNotFoundException(int code)
            : base(code) { }

        public HNotFoundException(string message)
            : base(message) { }

        public HNotFoundException(string message, int code)
            : base(message, code) { }

        public HNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
