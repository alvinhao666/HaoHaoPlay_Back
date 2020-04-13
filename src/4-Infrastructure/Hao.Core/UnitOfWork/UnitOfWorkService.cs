using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Core
{
    public class UnitOfWorkService: IUnitOfWorkService
    {
        [AttributeUsage(AttributeTargets.Method)]
        protected internal class UseTransactionAttribute : Attribute
        {
        }
    }
}
