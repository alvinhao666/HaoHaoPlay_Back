using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HCap
    {
        public static IServiceCollection AddCapService(this IServiceCollection services, string postgresqlConnection, string rabbitMQ_HostName, string rabbitMQ_VirtualHost, int rabbitMQ_Port, string rabbitMQ_UserName, string rabbitMQ_Password)
        {
            services.AddCap(x =>
            {
                //x.UseDashboard(); 

                x.UsePostgreSql(cfg => { cfg.ConnectionString = postgresqlConnection; });

                x.UseRabbitMQ(cfg =>
                {
                    cfg.HostName = rabbitMQ_HostName;
                    cfg.VirtualHost = rabbitMQ_VirtualHost; // 相当于数据库 可以在rabbitmq管理后台里面进行添加
                    cfg.Port = rabbitMQ_Port;
                    cfg.UserName = rabbitMQ_UserName;
                    cfg.Password = rabbitMQ_Password;
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

