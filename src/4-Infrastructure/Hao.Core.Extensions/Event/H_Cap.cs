using DotNetCore.CAP.Dashboard;
using Hao.Core.Extensions;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class H_Cap
    {
        public static IServiceCollection AddCapService(this IServiceCollection services, H_CapConfig config)
        {
            services.AddCap(x =>
            {
                x.UseDashboard(a =>
                {
                    a.Authorization = new[] { new CapDashboardFilter() };
                });  //默认地址http://localhost:8000/cap?password=666666

                x.UsePostgreSql(cfg => { cfg.ConnectionString = config.PostgreSqlConnection; });

                x.UseRabbitMQ(cfg =>
                {
                    cfg.HostName = config.HostName;
                    cfg.VirtualHost = config.VirtualHost; // 相当于数据库 可以在rabbitmq管理后台里面进行添加
                    cfg.Port = config.Port;
                    cfg.UserName = config.UserName;
                    cfg.Password = config.Password;
                });

                x.FailedRetryCount = 10; //失败重试机会
                x.FailedRetryInterval = 10; //每10秒重试一次
                //x.SucceedMessageExpiredAfter = 24 * 3600;
                // If you are using Kafka, you need to add the configuration：
                //x.UseKafka("localhost");
            });

            return services;
        }
    }

    internal class CapDashboardFilter : IDashboardAuthorizationFilter
    {

        public async Task<bool> AuthorizeAsync(DashboardContext context)
        {
            return await Task.Factory.StartNew(() =>
            {
                if (context.Request.GetQuery("password") == "666666")
                {
                    return true;
                }
                return false;
            });
        }
    }
}

