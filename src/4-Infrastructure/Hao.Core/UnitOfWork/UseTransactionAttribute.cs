using AspectCore.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hao.Core
{
    public class UseTransactionAttribute : AbstractInterceptorAttribute
    {
        public IUnitOfWork UnitOfWork { get; set; }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {

            try
            {
#if DEBUG
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("开始事务");
#endif
                UnitOfWork.BeginTran();
                await next(context);
#if DEBUG
                Console.WriteLine("提交事务");
#endif
                UnitOfWork.CommitTran();
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine("回滚事务");
#endif
                UnitOfWork.RollbackTran();
                throw ex;
            }
        }
    }
}
