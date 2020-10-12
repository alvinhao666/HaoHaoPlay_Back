using Hao.Dependency;
using System;
using System.Collections.Generic;
using System.Reflection;

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
                    .AddClasses(x => x.AssignableTo(typeof(ITransientDependency))) //直接或间接实现了ITransientDependency
                    .AsImplementedInterfaces()
                    .WithTransientLifetime())
                
                .Scan(scan => scan.FromAssembliesOf(types)
                    .AddClasses(x => x.AssignableTo(typeof(IScopeDependency)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime())
                
                .Scan(scan => scan.FromAssembliesOf(types)
                    .AddClasses(x => x.AssignableTo(typeof(ISingletonDependency)))
                    .AsImplementedInterfaces()
                    .WithSingletonLifetime());

            return services;
        }

        /// <summary>
        /// 自动IOC扫描注入
        /// </summary>
        public static IServiceCollection AutoDependency(this IServiceCollection services,
            IEnumerable<Assembly> assemblies)
        {
            services.Scan(scan => scan.FromAssemblies(assemblies)
                    .AddClasses(x => x.AssignableTo(typeof(ITransientDependency))) //直接或间接实现了ITransientDependency
                    .AsImplementedInterfaces()
                    .WithTransientLifetime())
                
                .Scan(scan => scan.FromAssemblies(assemblies)
                    .AddClasses(x => x.AssignableTo(typeof(IScopeDependency)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime())
                
                .Scan(scan => scan.FromAssemblies(assemblies)
                    .AddClasses(x => x.AssignableTo(typeof(ISingletonDependency)))
                    .AsImplementedInterfaces()
                    .WithSingletonLifetime());
            return services;
        }
    }
}