using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Core
{
    public static class ServiceLocator
    {
        //https://www.cnblogs.com/Leo_wl/p/6111503.html  从两个不同的ServiceProvider说起

        public static IServiceProvider ServiceProvider { get; private set; }


        public static void SetServiceProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public static T Resolve<T>()
        {
            return (T)ServiceProvider.GetService(typeof(T));
        }
    }

    ///// <summary>
    ///// 用于在静态/扩展类中使用某些服务类的对象实例
    ///// </summary>
    //public class ServiceLocator
    //{
    //    public static IServiceProvider ServiceProvider { get; private set; } //与aspectcore中scope对象不一样，需注意，single也不一样，只适用于瞬时对象

    //    public static void SetServiceProvider(IServiceProvider serviceProvider)
    //    {
    //        ServiceProvider = serviceProvider;
    //    }
    //}
}
