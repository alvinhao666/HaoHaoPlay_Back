using System;

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
