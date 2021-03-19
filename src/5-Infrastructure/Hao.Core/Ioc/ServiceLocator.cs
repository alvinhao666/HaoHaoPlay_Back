using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Core
{
    /// <summary>
    /// 用于在静态/扩展类中使用某些服务类的对象实例，只适用于瞬时对象， SnowflakeIdMaker(单例 且 在整个项目中 唯一 一个地方使用)
    /// </summary>
    public static class ServiceLocator
    {
        //https://www.cnblogs.com/Leo_wl/p/6111503.html  从两个不同的ServiceProvider说起

        public static IServiceProvider ServiceProvider { get; private set; }


        public static void Set(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public static T Resolve<T>()
        {
            return (T)ServiceProvider.GetService(typeof(T));
        }
    }
}
