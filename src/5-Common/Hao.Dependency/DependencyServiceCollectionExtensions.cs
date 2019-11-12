using System;
using System.Collections.Generic;
using System.Linq;
using Hao.Dependency;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyServiceCollectionExtensions
    {
        /// <summary>
        /// 自动IOC扫描注入
        /// </summary>
        public static IServiceCollection AutoDependency(this IServiceCollection services, IEnumerable<Type> types)
        {
            services.Scan(scan => scan.FromAssembliesOf(types)
                .AddClasses()
                .AsMatchingInterface((x, p) => p.AssignableTo<ITransientDependency>())
                .WithTransientLifetime()).
            Scan(scan => scan.FromAssembliesOf(types)
                .AddClasses()
                .AsMatchingInterface((x, p) => p.AssignableTo<ISingletonDependency>())
                .WithSingletonLifetime());
            return services;
        }

        /// <summary>
        /// 自动IOC扫描注入
        /// </summary>
        public static IServiceCollection AutoDependency(this IServiceCollection services, params Type[] types)
        {
            return services.AutoDependency(types.ToList());
        }
    }
}
