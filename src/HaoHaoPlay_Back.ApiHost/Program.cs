using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Hao.Core;
using Hao.Core.Extensions;
using Hao.Dependency;
using Hao.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System.Linq;
using System.Reflection;

namespace HaoHaoPlay.ApiHost
{
    public class Program
    {

        public static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterType<HTransactionAop>();

                    //InstancePerLifetimeScope：同一个Lifetime生成的对象是同一个实例；
                    //SingleInstance：单例模式，每次调用，都会使用同一个实例化的对象；每次都用同一个对象；
                    //InstancePerDependency：默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
                    builder.RegisterAssemblyTypes(Assembly.Load("Hao.Repository"), Assembly.Load("Hao.Core"))
                        .Where(m => typeof(ITransientDependency).IsAssignableFrom(m) && m != typeof(ITransientDependency)) //直接或间接实现了ITransientDependency
                        .AsImplementedInterfaces().InstancePerLifetimeScope().PropertiesAutowired();

                    builder.RegisterAssemblyTypes(Assembly.Load("Hao.AppService"), Assembly.Load("Hao.Event"))
                            .Where(m => typeof(ITransientDependency).IsAssignableFrom(m) && m != typeof(ITransientDependency))
                            .AsImplementedInterfaces().InstancePerLifetimeScope().PropertiesAutowired().EnableInterfaceInterceptors().InterceptedBy(typeof(HTransactionAop));
                    //一定要在你注入的服务后面加上EnableInterfaceInterceptors来开启你的拦截(aop)


                    var types = typeof(LoginController).Assembly.GetExportedTypes().Where(type => typeof(ControllerBase).IsAssignableFrom(type)).ToArray();
                    builder.RegisterTypes(types).PropertiesAutowired();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureLogging((hostingContext, logging) =>
                    {
                        logging.ClearProviders();
                        logging.SetMinimumLevel(LogLevel.Information);
                        logging.AddConsole();
                        logging.AddNLog($"NLog.{hostingContext.HostingEnvironment.EnvironmentName}.config");
                    })
                    .UseNLog()
                    .UseUrls("http://*:8000")
                    .UseKestrel()
                    .UseStartup<Startup>();
                })
                .Build()
                .Run();
        }
    }
}