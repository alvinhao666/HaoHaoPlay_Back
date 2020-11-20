using FreeSql;
using FreeSql.Internal;
using Hao.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class FreeSqlCollectionExtensions
    {
        /// <summary>
        /// orm服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="dbType"></param>
        /// <param name="connectionString"></param>
        /// <param name="slaveConnectionStrings"></param>
        /// <returns></returns>
        public static IServiceCollection AddOrmService(this IServiceCollection services, DataType dbType, string connectionString, params string[] slaveConnectionStrings)
        {
            IFreeSql fsql = new FreeSqlBuilder()
                            .UseConnectionString(dbType, connectionString)
                            .UseSlave(slaveConnectionStrings)
                            .UseNameConvert(NameConvertType.ToLower)
                            .UseMonitorCommand(cmd => {
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                Console.Write("SQL：");
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine(cmd.CommandText);
                                Console.ForegroundColor = ConsoleColor.White;
                            })
                            .UseNoneCommandParameter(true)
                            .Build();

            services.AddScoped<IFreeSqlContext>(a => new FreeSqlContext(fsql));

            return services;
        }

    }
}
