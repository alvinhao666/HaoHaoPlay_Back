using System;
using Hao.Snowflake;
using Hao.Snowflake.Redis;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SnowflakeDependencyInjection
    {
        public static IServiceCollection AddSnowflakeWithRedis(this IServiceCollection services, Action<RedisOptions> options)
        {
            services.Configure(options);
            services.AddSingleton<ISnowflakeIdMaker, SnowflakeIdMaker>();
            services.AddSingleton<IRedisClient, RedisClient>();
            services.AddSingleton<IDistributedSupport, DistributedSupportWithRedis>();
            services.AddHostedService<SnowflakeBackgroundServices>();
            return services;
        }

        //https://github.com/fuluteam/ICH.Snowflake

        //public static IServiceCollection AddSnowflake(this IServiceCollection services, Action<SnowflakeOption> option)
        //{
        //    services.Configure(option);
        //    services.AddSingleton<ISnowflakeIdMaker, SnowflakeIdMaker>();
        //    return services;
        //}
    }
}