using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Core
{
    public abstract class HEventHandler:IHEventHanlder
    {
        [AttributeUsage(AttributeTargets.Method)]
        protected internal class UseTransactionAttribute : Attribute
        {
        }
    }
}
