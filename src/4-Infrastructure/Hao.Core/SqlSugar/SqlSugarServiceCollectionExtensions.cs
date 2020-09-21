using Hao.Core;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public static IServiceCollection AddPostgreSqlService(this IServiceCollection services, string connectionString, Dictionary<string, int> slaveConnectionStrings)
        {
            return AddOrmService(services, DbType.PostgreSQL, connectionString, slaveConnectionStrings);
        }

        /// <summary>
        /// mysql
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <param name="slaveConnectionStrings"></param>
        /// <returns></returns>
        public static IServiceCollection AddMySqlService(this IServiceCollection services, string connectionString, Dictionary<string, int> slaveConnectionStrings)
        {
            return AddOrmService(services, DbType.MySql, connectionString, slaveConnectionStrings);
        }

        /// <summary>
        /// sqlserver
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <param name="slaveConnectionStrings"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlServerService(this IServiceCollection services, string connectionString, Dictionary<string, int> slaveConnectionStrings)
        {
            return AddOrmService(services, DbType.SqlServer, connectionString, slaveConnectionStrings);
        }

        /// <summary>
        /// oracle
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <param name="slaveConnectionStrings"></param>
        /// <returns></returns>
        public static IServiceCollection AddOracleService(this IServiceCollection services, string connectionString, Dictionary<string, int> slaveConnectionStrings)
        {
            return AddOrmService(services, DbType.Oracle, connectionString, slaveConnectionStrings);
        }

        /// <summary>
        /// orm服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="dbType"></param>
        /// <param name="connectionString"></param>
        /// <param name="slaveConnectionStrings"></param>
        /// <returns></returns>
        private static IServiceCollection AddOrmService(IServiceCollection services, DbType dbType, string connectionString, Dictionary<string, int> slaveConnectionStrings)
        {
            var connectionConfig = new ConnectionConfig()
            {
                ConnectionString = connectionString,
                DbType = dbType,
                IsAutoCloseConnection = true,//开启自动释放模式和EF原理一样 自动释放数据务，如果存在事务，在事务结束后释放
                InitKeyType = InitKeyType.Attribute,  //SysTable  表示通过数据库系统表查询表主键，这种需要数据库最高权限，并且数据库表需有主键或能获取到主键。   Attribute 表示通过实体  [SugarColumn(IsPrimaryKey = true)]标签获取主键，而无需通过数据库表。
                ConfigureExternalServices = new ConfigureExternalServices()
                {
                    DataInfoCacheService = new SqlSugarRedisCache() //RedisCache是继承ICacheService自已实现的一个类
                }
                //MoreSettings = new ConnMoreSettings(){ PgSqlIsAutoToLower = false}
                //config.SlaveConnectionConfigs = new List<SlaveConnectionConfig>() { //= 如果配置了 SlaveConnectionConfigs那就是主从模式,所有的写入删除更新都走主库，查询走从库，事务内都走主库，HitRate表示权重 值越大执行的次数越高，如果想停掉哪个连接可以把HitRate设为0 
                //new SlaveConnectionConfig() { HitRate=10, ConnectionString=Config.ConnectionString2 },
                //new SlaveConnectionConfig() { HitRate=30, ConnectionString=Config.ConnectionString3 }
            };

            if (slaveConnectionStrings?.Count() > 0)
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

            connectionConfig.AopEvents = new AopEvents
            {
#if DEBUG
                OnLogExecuting = (sql, pars) =>
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("SQL：");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(LookSQL(sql, pars).ToString());
                    Console.ForegroundColor = ConsoleColor.White;
                },
#endif
                OnError = (exp) =>//执行SQL 错误事件
                {
                    //exp.sql exp.parameters 可以拿到参数和错误Sql  
                    StringBuilder sb_SugarParameterStr = new StringBuilder("###SugarParameter参数为:");
                    var parametres = exp.Parametres as SugarParameter[];
                    if (parametres != null)
                    {
                        sb_SugarParameterStr.Append(JsonConvert.SerializeObject(parametres));
                    }

                    StringBuilder sb_error = new StringBuilder();
                    sb_error.AppendLine("SqlSugarClient执行sql异常信息:" + exp.Message);
                    sb_error.AppendLine("###赋值后sql:" + LookSQL(exp.Sql, parametres));
                    sb_error.AppendLine("###带参数的sql:" + exp.Sql);
                    sb_error.AppendLine("###参数信息:" + sb_SugarParameterStr.ToString());
                    sb_error.AppendLine("###StackTrace信息:" + exp.StackTrace);

                    throw new Exception(sb_error.ToString());
                },
                OnExecutingChangeSql = (sql, pars) => //SQL执行前 可以修改SQL
                {
                    //判断update或delete方法是否有where条件。如果真的想删除所有数据，请where(p=>true)或where(p=>p.id>0)
                    if (sql.TrimStart().IndexOf("delete ", StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        if (sql.IndexOf("where", StringComparison.CurrentCultureIgnoreCase) <= 0)
                        {
                            throw new Exception("delete删除方法需要有where条件！！");
                        }
                    }
                    else if (sql.TrimStart().IndexOf("update ", StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        if (sql.IndexOf("where", StringComparison.CurrentCultureIgnoreCase) <= 0
                            && sql.IndexOf(" join ", StringComparison.CurrentCultureIgnoreCase) <= 0 //连表查询
                            )
                        {
                            throw new Exception("update更新方法需要有where条件或连表更新！！");
                        }
                    }

                    return new KeyValuePair<string, SugarParameter[]>(sql, pars);
                }
            };


            services.AddScoped<ISqlSugarClient>(o => new SqlSugarClient(connectionConfig));

            return services;
        }

        /// <summary>
        /// 查看赋值后的sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars">参数</param>
        /// <returns></returns>
        private static string LookSQL(string sql, SugarParameter[] pars)
        {
            if (pars == null || pars.Length == 0) return sql;

            StringBuilder sb_sql = new StringBuilder(sql);
            var tempOrderPars = pars.Where(p => p.Value != null).OrderByDescending(p => p.ParameterName.Length).ToList();//防止 @par1错误替换@par12
            for (var index = 0; index < tempOrderPars.Count; index++)
            {
                sb_sql.Replace(tempOrderPars[index].ParameterName, "'" + tempOrderPars[index].Value.ToString() + "'");
            }

            return sb_sql.ToString();
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
