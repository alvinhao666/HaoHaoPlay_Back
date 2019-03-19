using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SqlSugar;

namespace Hao.DbContext
{
    /// <summary>
    /// SqlSugar 注入Service的扩展方法 <see cref="IServiceCollection" />.
    /// SqlSugar 和 Dbcontext 不同，没有DbContext池 所有的 AddSQLSugarClientPool 已全部删除
    /// SqlSugarClient 没有无参的构造函数 configAction 可为空的方法已注释
    /// </summary>
    public static class SqlSugarServiceCollectionExtensions
    {

        public static IServiceCollection AddSqlSugarClient(this IServiceCollection serviceCollection, Action<ConnectionConfig> configAction, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime configLifetime = ServiceLifetime.Scoped)
        {
            if (contextLifetime == ServiceLifetime.Singleton)
            {
                configLifetime = ServiceLifetime.Singleton;
            }

            serviceCollection.TryAdd(new ServiceDescriptor(typeof(ConnectionConfig), p => ConnectionConfigFactory(configAction), configLifetime));

            serviceCollection.TryAdd(new ServiceDescriptor(typeof(SqlSugarClient), typeof(SqlSugarClient), contextLifetime));

            return serviceCollection;
        }


        private static ConnectionConfig ConnectionConfigFactory(Action<ConnectionConfig> configAction)
        {
            var config = new ConnectionConfig();

            configAction.Invoke(config);

            return config;
        }
    }
}
