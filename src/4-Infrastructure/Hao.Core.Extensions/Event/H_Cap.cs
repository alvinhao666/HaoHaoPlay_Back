using Hao.Core.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class H_Cap
    {
        public static IServiceCollection AddCapService(this IServiceCollection services, H_CapConfig config )
        {
            services.AddCap(x =>
            {
                x.UseDashboard(); 

                x.UsePostgreSql(cfg => { cfg.ConnectionString = config.PostgreSqlConnection; });

                x.UseRabbitMQ(cfg =>
                {
                    cfg.HostName = config.HostName;
                    cfg.VirtualHost = config.VirtualHost; // 相当于数据库 可以在rabbitmq管理后台里面进行添加
                    cfg.Port = config.Port;
                    cfg.UserName = config.UserName;
                    cfg.Password = config.Password;
                });

                x.FailedRetryCount = 2; //失败重试机会
                x.FailedRetryInterval = 5;
                x.SucceedMessageExpiredAfter = 24 * 3600;
                // If you are using Kafka, you need to add the configuration：
                //x.UseKafka("localhost");
            });

            return services;
        }
    }
}

