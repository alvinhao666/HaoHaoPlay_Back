using Hao.Core;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SqlSugarServiceCollectionExtensions
    {
        /// <summary>
        /// postgresql
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <param name="slaveConnectionStrings"></param>
        /// <returns></returns>
        public static IServiceCollection AddPostgreSQLService(this IServiceCollection services, string connectionString, Dictionary<string, int> slaveConnectionStrings = null)
        {
            return AddSQLService(services, DbType.PostgreSQL, connectionString, slaveConnectionStrings);
        }

        /// <summary>
        /// mysql
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <param name="slaveConnectionStrings"></param>
        /// <returns></returns>
        public static IServiceCollection AddMySQLService(this IServiceCollection services, string connectionString, Dictionary<string, int> slaveConnectionStrings = null)
        {
            return AddSQLService(services, DbType.MySql, connectionString, slaveConnectionStrings);
        }

        /// <summary>
        /// orm服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="dbType"></param>
        /// <param name="connectionString"></param>
        /// <param name="slaveConnectionStrings"></param>
        /// <returns></returns>
        private static IServiceCollection AddSQLService(IServiceCollection services, DbType dbType, string connectionString, Dictionary<string, int> slaveConnectionStrings = null)
        {
            var connectionConfig = new ConnectionConfig()
            {
                ConnectionString = connectionString,
                DbType = dbType,
                IsAutoCloseConnection = true,//开启自动释放模式和EF原理一样 自动释放数据务，如果存在事务，在事务结束后释放
                InitKeyType = InitKeyType.SystemTable,  //如果不是SA等高权限数据库的账号,需要从实体读取主键或者自增列 InitKeyType要设成Attribute (不需要读取这些信息)
                ConfigureExternalServices = new ConfigureExternalServices()
                {
                    DataInfoCacheService = new SqlSugarRedisCache() //RedisCache是继承ICacheService自已实现的一个类
                }
                //MoreSettings = new ConnMoreSettings(){ PgSqlIsAutoToLower = false}
                //config.SlaveConnectionConfigs = new List<SlaveConnectionConfig>() { //= 如果配置了 SlaveConnectionConfigs那就是主从模式,所有的写入删除更新都走主库，查询走从库，事务内都走主库，HitRate表示权重 值越大执行的次数越高，如果想停掉哪个连接可以把HitRate设为0 
                //new SlaveConnectionConfig() { HitRate=10, ConnectionString=Config.ConnectionString2 },
                //new SlaveConnectionConfig() { HitRate=30, ConnectionString=Config.ConnectionString3 }
            };

            if (slaveConnectionStrings != null && slaveConnectionStrings.Count() > 0)
            {
                connectionConfig.SlaveConnectionConfigs = new List<SlaveConnectionConfig>();
                foreach (var item in slaveConnectionStrings)
                {
                    connectionConfig.SlaveConnectionConfigs.Add(new SlaveConnectionConfig()
                    {
                        ConnectionString = item.Key,
                        HitRate = item.Value
                    });
                }
            }
#if DEBUG
            connectionConfig.AopEvents = new AopEvents
            {
                OnLogExecuting = (sql, p) =>
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("SQL：");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(sql);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("参数：");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(string.Join(",", p?.Select(it => string.Format("{0}:{1}", it.ParameterName, it.Value))));
                    Console.ForegroundColor = ConsoleColor.White;
                }
            };
#endif
            services.AddScoped<ISqlSugarClient>(o => new SqlSugarClient(connectionConfig));

            return services;
        }




        //public static IServiceCollection AddSqlSugarClient(this IServiceCollection services, Action<ConnectionConfig> configAction, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime configLifetime = ServiceLifetime.Scoped)
        //{
        //    services.TryAdd(new ServiceDescriptor(typeof(ConnectionConfig), p => ConnectionConfigFactory(configAction), configLifetime));

        //    services.TryAdd(new ServiceDescriptor(typeof(ISqlSugarClient), typeof(SqlSugarClient), contextLifetime));

        //    return services;
        //}

        //private static ConnectionConfig ConnectionConfigFactory(Action<ConnectionConfig> configAction)
        //{
        //    var config = new ConnectionConfig();

        //    configAction.Invoke(config);

        //    return config;
        //}
    }
}
