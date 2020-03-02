using Hao.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Enum
{
    public enum PasswordLevel
    {
        [HDescription("弱")]
        Weak,
        [HDescription("中")]
        Medium,
        [HDescription("强")]
        Strong
    }
}
