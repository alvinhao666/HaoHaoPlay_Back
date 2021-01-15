using System;
using System.Collections.Generic;

namespace Hao.Core
{
    public class H_Exception : Exception
    {
        public int? Code { get; private set; }

        public H_Exception() { }


        public H_Exception(string message) : base(message) { }


        public H_Exception(string message, int? code) : base(message)
        {
            this.Code = code;
        }

        public H_Exception(KeyValuePair<int, string> pair) : base(pair.Value)
        {
            this.Code = pair.Key;
        }
    }


    public static class H_AssertEx
    {
        public static void That(bool condition, string msg) 
        {
            if (condition)
            {
               throw new H_Exception(msg);
            }
        }
    }
}
