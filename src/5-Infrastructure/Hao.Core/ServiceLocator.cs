using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Core
{
    /// <summary>
    /// 用于在静态/扩展类中使用某些服务类的对象实例
    /// </summary>
    public class ServiceLocator
    {
        public static IServiceProvider ServiceProvider { get; private set; } //与aspectcore中scope对象不一样，需注意，single也不一样

        public static void SetServiceProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
    }
}
