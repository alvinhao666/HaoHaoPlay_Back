using SqlSugar;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SqlSugarServiceCollectionExtensions
    {

        public static IServiceCollection AddSqlSugarClient(this IServiceCollection services, ConnectionConfig config)
        {

            services.AddScoped<ISqlSugarClient>(o => new SqlSugarClient(config));

            return services;
        }


        //public static IServiceCollection AddSqlSugarClient(this IServiceCollection services, Action<ConnectionConfig> configAction, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime configLifetime = ServiceLifetime.Scoped)
        //{
        //    services.TryAdd(new ServiceDescriptor(typeof(ConnectionConfig), p => ConnectionConfigFactory(configAction), configLifetime));

        //    services.TryAdd(new ServiceDescriptor(typeof(ISqlSugarClient), typeof(SqlSugarClient), contextLifetime));

        //    return services;
        //}

        //private static ConnectionConfig ConnectionConfigFactory(Action<ConnectionConfig> configAction)
        //{
        //    var config = new ConnectionConfig();

        //    configAction.Invoke(config);

        //    return config;
        //}

    }
}
