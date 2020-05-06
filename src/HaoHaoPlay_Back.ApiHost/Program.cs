using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hao.Dependency;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System.Linq;
using System.Reflection;
using AspectCore.Extensions.Autofac;
using Hao.WebApi.Controllers;

namespace HaoHaoPlay.ApiHost
{
    public class Program
    {

        public static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory(builder=>
                {
                    //InstancePerLifetimeScope：同一个Lifetime生成的对象是同一个实例；
                    //SingleInstance：单例模式，每次调用，都会使用同一个实例化的对象；每次都用同一个对象；
                    //InstancePerDependency：默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；

                    builder.RegisterAssemblyTypes(
                        Assembly.Load("Hao.Core"), 
                        Assembly.Load("Hao.Repository"),
                        Assembly.Load("Hao.Event"),
                        Assembly.Load("Hao.AppService"))
                        .Where(m => typeof(ITransientDependency).IsAssignableFrom(m) && m != typeof(ITransientDependency)) //直接或间接实现了ITransientDependency
                        .AsImplementedInterfaces().InstancePerDependency().PropertiesAutowired();

                    var types = typeof(LoginController).Assembly.GetExportedTypes().Where(type => typeof(ControllerBase).IsAssignableFrom(type)).ToArray();
                    builder.RegisterTypes(types).PropertiesAutowired();

                    //调用RegisterDynamicProxy扩展方法在Autofac中注册动态代理服务和动态代理配置 aop
                    //在一般情况下可以使用抽象的AbstractInterceptorAttribute自定义特性类，它实现IInterceptor接口。AspectCore默认实现了基于Attribute的拦截器配置
                    builder.RegisterDynamicProxy();
                }))
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