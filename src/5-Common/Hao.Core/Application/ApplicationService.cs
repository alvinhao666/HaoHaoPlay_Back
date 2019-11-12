using System;

namespace Hao.Core
{
    public abstract class ApplicationService: IApplicationService 
    {
        [AttributeUsage(AttributeTargets.Method)]
        protected internal class UseTransactionAttribute : Attribute
        {
        }
    }
}
