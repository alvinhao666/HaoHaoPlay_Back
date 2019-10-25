using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hao.Core.Application
{
    public abstract class ApplicationService: IApplicationService 
    {
        [AttributeUsage(AttributeTargets.Method)]
        protected internal class UseTransactionAttribute : Attribute
        {

        }
    }
}
