using FreeSql;
using FreeSql.Aop;
using FreeSql.Internal;
using Hao.Core;
using Hao.Snowflake;
using Hao.Utility;
using System;
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
              //.ApplyOnlyIf<IsCreateAudited>(nameof(IsCreateAudited), () => CurrentUser.Value != null, x => x.CreatorId == CurrentUser.Value.Id);

#if DEBUG
            fsql.Aop.CurdAfter += (s, e) => Aop_CurdAfter(s, e);
#endif

            fsql.Aop.AuditValue += (s, e) => Aop_AuditValue(s, e, services);


            services.AddScoped<IFreeSqlContext>(a => new FreeSqlContext(fsql));

            return services;
        }


        private static void Aop_CurdAfter(object sender, CurdAfterEventArgs e)
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
            Console.WriteLine($"执行时间：{e.ElapsedMilliseconds}毫秒");
            Console.WriteLine($"{e.Sql}");
            if (e.DbParms.Length > 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                foreach (var item in e.DbParms)
                {
                    Console.Write($"{item.ParameterName}".PadRight(30, ' '));
                    Console.WriteLine($"{item.Value}");
                }
            }

            if (e.Exception != null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"错误信息:{e.Exception.Message}");
            }
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("<<<<<<<<<<<<<<< 数据库日志 END >>>>>>>>>>>>>>>>");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }


        private static void Aop_AuditValue(object sender, AuditValueEventArgs e, IServiceCollection services)
        {
            if (e.AuditValueType == AuditValueType.Insert)
            {
                switch (e.Property.Name)
                {
                    case "Id":
                        if (e.Column.CsType == H_Util.GuidType)
                        {
                            e.Value = Guid.NewGuid();
                        }
                        else if (e.Column.CsType == H_Util.LongType)
                        {
                            var idWorker = services.BuildServiceProvider().GetService<IdWorker>();
                            e.Value = idWorker.NextId();
                        }
                        break;

                    case nameof(IsCreateAudited.CreatorId):
                        if (CurrentUser.Value?.Id != null) e.Value = CurrentUser.Value.Id;
                        break;

                    case nameof(IsCreateAudited.CreateTime):
                        if (CurrentUser.Value?.Id != null) e.Value = DateTime.Now;
                        break;
                }
            }
            else if (e.AuditValueType == AuditValueType.Update)
            {
                if (CurrentUser.Value?.Id != null)
                {
                    switch (e.Property.Name)
                    {
                        case nameof(IsModifyAudited.ModifierId):
                            if (CurrentUser.Value?.Id != null) e.Value = CurrentUser.Value.Id;
                            break;

                        case nameof(IsModifyAudited.ModifyTime):
                            if (CurrentUser.Value?.Id != null) e.Value = DateTime.Now;
                            break;
                    }
                }
            }
        }
    }
}
