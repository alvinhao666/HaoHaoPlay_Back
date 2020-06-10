using AspectCore.DynamicProxy;
using SqlSugar;
using System;
using System.Threading.Tasks;

namespace Hao.Core
{
    public abstract class ApplicationService: IApplicationService 
    {
        [AttributeUsage(AttributeTargets.Method)]
        protected class UnitOfWorkAttribute : AbstractInterceptorAttribute
        {
            public override async Task Invoke(AspectContext context, AspectDelegate next)
            {
                var sqlSugarClient = context.ServiceProvider.GetService(typeof(ISqlSugarClient)) as ISqlSugarClient;
                try
                {
#if DEBUG
                    Console.ForegroundColor = ConsoleColor.Magenta;

                    Console.WriteLine("开始事务：" + sqlSugarClient.ContextID);
#endif
                    sqlSugarClient.Ado.BeginTran();
                    await next(context);
#if DEBUG
                    Console.ForegroundColor = ConsoleColor.Magenta;

                    Console.WriteLine("提交事务：" + sqlSugarClient.ContextID);
#endif
                    sqlSugarClient.Ado.CommitTran();
                }
                catch (Exception ex)
                {
#if DEBUG
                    Console.ForegroundColor = ConsoleColor.Blue;

                    Console.WriteLine("回滚事务：" + sqlSugarClient.ContextID);
#endif
                    sqlSugarClient.Ado.RollbackTran();
                    throw ex;
                }
            }
        }
    }
}
