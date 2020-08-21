using AspectCore.DynamicProxy;
using Hao.RunTimeException;
using Microsoft.Extensions.Configuration;
using NLog;
using SqlSugar;
using System;
using System.Threading.Tasks;

namespace Hao.Core
{
    public abstract class BaseService
    {
        protected readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 工作单元，事务，原子操作
        /// </summary>
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


        /// <summary>
        /// 分布式锁，防并发
        /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        protected class DistributedLockAttribute : AbstractInterceptorAttribute
        {
            protected readonly ILogger Logger = LogManager.GetCurrentClassLogger();

            private string _lockName;

            private int _timeoutSeconds;

            private bool _autoDelay;

            public DistributedLockAttribute(string lockName, int timeoutSeconds = 10, bool autoDelay = true)
            {
                _lockName = lockName;
                _timeoutSeconds = timeoutSeconds;
                _autoDelay = autoDelay;
            }

            public override async Task Invoke(AspectContext context, AspectDelegate next)
            {
                var config = context.ServiceProvider.GetService(typeof(IConfiguration)) as IConfiguration;

                var prefix = config["RedisPrefix:Lock"];

                if (string.IsNullOrWhiteSpace(prefix)) throw new H_Exception("请配置分布式锁名称的前缀字符");

                using (var redisLock = RedisHelper.Lock(prefix + _lockName, _timeoutSeconds, _autoDelay))
                {
                    if (redisLock == null)
                    {
                        Logger.Error("系统异常：开启分布式锁失败");
                        throw new H_Exception("系统异常");
                    }

                    //if (redisLock == null) throw new H_Exception("系统异常"); //开启分布式锁超时 //对象为null，不占资源 ，编译后的代码没有fianlly,不执行dispose()方法
                    //锁超时是什么意思呢？如果一个得到锁的线程在执行任务的过程中挂掉，来不及显式地释放锁，这块资源将会永远被锁住，别的线程再也别想进来。

                    //所以，setnx的key必须设置一个超时时间，以保证即使没有被显式释放，这把锁也要在一定时间后自动释放。
                    await next(context);
                }
            }
        }
    }
}
