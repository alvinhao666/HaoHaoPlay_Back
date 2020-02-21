using Hao.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HCap
    {
        public static IServiceCollection AddCapService(this IServiceCollection services, HCapParam param )
        {
            services.AddCap(x =>
            {
                x.UseDashboard(); 

                x.UsePostgreSql(cfg => { cfg.ConnectionString = param.PostgreSqlConnection; });

                x.UseRabbitMQ(cfg =>
                {
                    cfg.HostName = param.HostName;
                    cfg.VirtualHost = param.VirtualHost; // 相当于数据库 可以在rabbitmq管理后台里面进行添加
                    cfg.Port = param.Port;
                    cfg.UserName = param.UserName;
                    cfg.Password = param.Password;
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

