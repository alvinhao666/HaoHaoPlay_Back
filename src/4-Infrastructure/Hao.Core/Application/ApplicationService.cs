using AspectCore.DynamicProxy;
using CSRedis;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.Threading.Tasks;

namespace Hao.Core
{
    public abstract class ApplicationService : IApplicationService
    {
        public IConfiguration Config { get; set; }

        [AttributeUsage(AttributeTargets.Method)]
        protected class UnitOfWorkAttribute : AbstractInterceptorAttribute
        {
            public override async Task Invoke(AspectContext context, AspectDelegate next)
            {
                var sqlSugarClient = context.ServiceProvider.GetService(typeof(ISqlSugarClient)) as ISqlSugarClient;
                try
                {
#if DEBUG
                    Console.ForegroundColor = ConsoleColor.Blue;

                    Console.WriteLine("开始事务：" + sqlSugarClient.ContextID);
#endif
                    sqlSugarClient.Ado.BeginTran();
                    await next(context);
#if DEBUG
                    Console.ForegroundColor = ConsoleColor.Blue;

                    Console.WriteLine("提交事务：" + sqlSugarClient.ContextID);
#endif
                    sqlSugarClient.Ado.CommitTran();
                }
                catch (Exception ex)
                {
#if DEBUG
                    Console.ForegroundColor = ConsoleColor.Red;

                    Console.WriteLine("回滚事务：" + sqlSugarClient.ContextID);
#endif
                    sqlSugarClient.Ado.RollbackTran();
                    throw ex;
                }
            }
        }


        protected CSRedisClientLock DistributedLock(string name, int timeoutSeconds = 10, bool autoDelay = true)
        {
            var prefix = Config["RedisPrefix:Lock"];

            if (string.IsNullOrWhiteSpace(prefix)) throw new Exception("分布式锁前缀字符不能为空");

            var redisLock = RedisHelper.Lock($"{prefix}{name}", timeoutSeconds, autoDelay);

            if (redisLock == null) throw new Exception("开启分布式锁异常");

            //if (redisLock == null) throw new H_Exception("系统异常"); //开启分布式锁超时 //对象为null，不占资源 ，编译后的代码没有fianlly,不执行dispose()方法
            return redisLock;
        }
    }
}
