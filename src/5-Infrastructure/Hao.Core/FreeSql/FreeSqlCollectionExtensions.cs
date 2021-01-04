using FreeSql;
using FreeSql.Internal;
using Hao.Core;
using Hao.Snowflake;
using Hao.Utility;
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

            fsql.GlobalFilter.ApplyOnly<IsSoftDelete>(nameof(IsSoftDelete), x => x.IsDeleted == false);
            
#if DEBUG
            fsql.Aop.CurdBefore += (s, e) =>
            {                 
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"{e.Sql}");
                if (e.DbParms.Length > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    foreach (var item in e.DbParms)
                    {
                        Console.Write($"{item.ParameterName}".PadRight(30,' '));
                        Console.WriteLine($"{item.Value}");
                    }
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
            };
#endif

            fsql.Aop.AuditValue += (s, e) =>
            {
                if (e.AuditValueType == FreeSql.Aop.AuditValueType.Insert)  //只处理id    操作人 操作时间 放仓储基类处理
                {
                    if (e.Property.Name == "Id")
                    {
                        if (e.Column.CsType == H_Util.GuidType)
                        {
                            e.Value = Guid.NewGuid();
                        }
                        else if (e.Column.CsType == H_Util.LongType)
                        {
                            var idWorker = services.BuildServiceProvider().GetService<IdWorker>();
                            e.Value = idWorker.NextId();
                        }
                    }
                }
            };

            services.AddScoped<IFreeSqlContext>(a => new FreeSqlContext(fsql));

            return services;
        }

    }
}
