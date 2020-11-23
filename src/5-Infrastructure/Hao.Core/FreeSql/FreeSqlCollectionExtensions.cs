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
                            // .UseMonitorCommand(cmd => {
                            //     Console.ForegroundColor = ConsoleColor.DarkYellow;
                            //     Console.Write("SQL：");
                            //     Console.ForegroundColor = ConsoleColor.Cyan;
                            //     Console.WriteLine(cmd.CommandText);
                            //     Console.ForegroundColor = ConsoleColor.White;
                            // })
                            //.UseNoneCommandParameter(true)
                            .Build();
#if DEBUG
            fsql.Aop.CurdBefore += (s, e) =>
            {                 
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("SQL：");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"{e.Sql}");
                if (e.DbParms.Length > 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("参数：");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    foreach (var item in e.DbParms)
                    {
                        Console.Write($"{item.ParameterName}     ");
                        Console.WriteLine($"{item.Value}");
                    }
                }
                Console.ForegroundColor = ConsoleColor.White;
            };
#endif


            services.AddScoped<IFreeSqlContext>(a => new FreeSqlContext(fsql));

            return services;
        }

    }
}
