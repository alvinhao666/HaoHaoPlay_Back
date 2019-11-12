using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Utility
{
    public static class BoolExtensions
    {
        public static bool IsTrue(this bool? flag)
        {
            return flag.HasValue && flag.Value;
        }
    }
}
