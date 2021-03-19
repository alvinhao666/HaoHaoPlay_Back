//using AspectCore.Configuration;
//using AspectCore.DependencyInjection;
//using AspectCore.Extensions.DependencyInjection;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Hao.Core
//{
//    public class AspectCoreContainer
//    {
//        private static IServiceResolver  _resolver;

//        public static IServiceProvider Build(IServiceCollection services)
//        {
//            if (services == null) throw new ArgumentNullException(nameof(services));

//            return _resolver = services.ToServiceContext().Build();
//        }

//        public static T Resolve<T>()
//        {
//            if (_resolver == null)
//                throw new ArgumentNullException(nameof(_resolver), "调用此方法时必须先调用Build！");
//            return _resolver.Resolve<T>();
//        }

//        public static object Resolve(Type type)
//        {
//            if (_resolver == null)
//                throw new ArgumentNullException(nameof(_resolver), "调用此方法时必须先调用Build！");
//            return _resolver.Resolve(type);
//        }

//        public static object Resolve(string typeName)
//        {
//            if (_resolver == null)
//                throw new ArgumentNullException(nameof(_resolver), "调用此方法时必须先调用Build！");
//            return _resolver.Resolve(Type.GetType(typeName));
//        }

//        public static List<T> ResolveServices<T>()
//        {
//            if (_resolver == null)
//                throw new ArgumentNullException(nameof(_resolver), "调用此方法时必须先调用Build！");
//            return _resolver.GetServices<T>().ToList();
//        }

//        public static List<T> ResolveServices<T>(Func<T, bool> filter)
//        {
//            if (_resolver == null)
//                throw new ArgumentNullException(nameof(_resolver), "调用此方法时必须先调用Build！");
//            if (filter == null) filter = m => true;
//            return ResolveServices<T>().Where(filter).ToList();
//        }
//    }
//}
