﻿using AspectCore.Configuration;
using AspectCore.DependencyInjection;
using AspectCore.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hao.Core
{
    public class AspectCoreContainer
    {
        private static IServiceResolver resolver { get; set; }

        public static IServiceProvider Build(IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            return resolver = services.ToServiceContext().Build();
        }

        public static T Resolve<T>()
        {
            if (resolver == null)
                throw new ArgumentNullException(nameof(resolver), "调用此方法时必须先调用BuildServiceProvider！");
            return resolver.Resolve<T>();
        }

        public static object Resolve(Type type)
        {
            if (resolver == null)
                throw new ArgumentNullException(nameof(resolver), "调用此方法时必须先调用BuildServiceProvider！");
            return resolver.Resolve(type);
        }

        public static object Resolve(string typeName)
        {
            if (resolver == null)
                throw new ArgumentNullException(nameof(resolver), "调用此方法时必须先调用BuildServiceProvider！");
            return resolver.Resolve(Type.GetType(typeName));
        }

        public static List<T> ResolveServices<T>()
        {
            if (resolver == null)
                throw new ArgumentNullException(nameof(resolver), "调用此方法时必须先调用BuildServiceProvider！");
            return resolver.GetServices<T>().ToList();
        }

        public static List<T> ResolveServices<T>(Func<T, bool> filter)
        {
            if (resolver == null)
                throw new ArgumentNullException(nameof(resolver), "调用此方法时必须先调用BuildServiceProvider！");
            if (filter == null) filter = m => true;
            return ResolveServices<T>().Where(filter).ToList();
        }
    }
}
