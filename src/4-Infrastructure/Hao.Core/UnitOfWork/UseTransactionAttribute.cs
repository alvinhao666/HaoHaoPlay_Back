using AspectCore.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AspectCore.DependencyInjection;
using SqlSugar;

namespace Hao.Core
{
    public class UseTransactionAttribute : AbstractInterceptorAttribute
    {
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var sqlSugarClient = context.ServiceProvider.GetService(typeof(ISqlSugarClient)) as ISqlSugarClient;
            try
            {
          
#if DEBUG
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("开始事务");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("事务" + sqlSugarClient.ContextID);
#endif
                sqlSugarClient.Ado.BeginTran();
                await next(context);
#if DEBUG
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("提交事务");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("事务" + sqlSugarClient.ContextID);
#endif
                sqlSugarClient.Ado.CommitTran();
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("回滚事务");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("事务" + sqlSugarClient.ContextID);
#endif
                sqlSugarClient.Ado.RollbackTran();
                throw ex;
            }
        }
    }
}
