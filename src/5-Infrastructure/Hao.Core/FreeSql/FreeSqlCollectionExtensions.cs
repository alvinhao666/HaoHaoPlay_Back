using FreeSql;
using FreeSql.Aop;
using FreeSql.Internal;
using Hao.Core;
using Hao.Snowflake;
using Hao.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Hao.Runtime;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class FreeSqlCollectionExtensions
    {
        internal static AsyncLocal<ICurrentUser> CurrentUser = new AsyncLocal<ICurrentUser>();
        
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

            fsql.GlobalFilter
                .ApplyOnly<IsSoftDelete>(nameof(IsSoftDelete), x => x.IsDeleted == false);
                // .ApplyOnlyIf<IsCreateAudited>(nameof(IsCreateAudited), () => CurrentUser.Value != null, x => x.CreatorId == CurrentUser.Value.Id);
            
#if DEBUG
            fsql.Aop.CurdAfter += (s, e) =>
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("<<<<<<<<<<<<<<< 数据库日志 BEGIN >>>>>>>>>>>>>>");
                var curdType = "查询";
                switch (e.CurdType)
                {
                    case CurdType.Select:
                        break;
                    case CurdType.Delete:
                        curdType = "删除";
                        break;
                    case CurdType.Update:
                        curdType = "更新";
                        break;
                    case CurdType.Insert:
                        curdType = "新增";
                        break;
                }

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"{curdType}实体：{e.EntityType.Name}");
                Console.WriteLine($"执行时间：{e.ElapsedMilliseconds}");
                Console.WriteLine($"{e.Sql}");
                if (e.DbParms.Length > 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    foreach (var item in e.DbParms)
                    {
                        Console.Write($"{item.ParameterName}".PadRight(30,' '));
                        Console.WriteLine($"{item.Value}");
                    }
                }
  
                
                if (e.Exception != null)
                {                
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"执行Sql错误:{e.Sql}");
                    Console.WriteLine($"错误信息:{e.Exception.Message}");
                }
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("<<<<<<<<<<<<<<< 数据库日志 END >>>>>>>>>>>>>>>");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
            };
#endif

            fsql.Aop.AuditValue += (s, e) =>
            {
                if (e.AuditValueType == AuditValueType.Insert)  //只处理id    操作人 操作时间 放仓储基类处理
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

                    if (CurrentUser.Value?.Id!=null)
                    {
                        if (e.Property.Name == nameof(IsCreateAudited.CreatorId)) e.Value = CurrentUser.Value?.Id;

                        if (e.Property.Name == nameof(IsCreateAudited.CreateTime)) e.Value = DateTime.Now;
                    }
                }

                if (e.AuditValueType == AuditValueType.Update)
                {
                    if (CurrentUser.Value?.Id != null)
                    {
                        if (e.Property.Name == nameof(IsModifyAudited.ModifierId)) e.Value = CurrentUser.Value?.Id;
                        
                        if (e.Property.Name == nameof(IsModifyAudited.ModifyTime)) e.Value = DateTime.Now;
                    }
                }
            };

            services.AddScoped<IFreeSqlContext>(a => new FreeSqlContext(fsql));

            return services;
        }

    }
}
