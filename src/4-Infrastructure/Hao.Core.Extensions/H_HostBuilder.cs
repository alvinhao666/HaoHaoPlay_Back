using AspectCore.Extensions.Autofac;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hao.Dependency;
using Hao.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Reflection;
using NLog.Web;

namespace Hao.Core.Extensions
{
    public class H_HostBuilder
    {
        /// <summary>
        /// 启动
        /// </summary>
        /// <typeparam name="TStartup"></typeparam>
        /// <param name="args"></param>
        public void Run<TStartup>(string[] args) where TStartup : H_Startup
        {
            CreateBuilder<TStartup>(args).Build().Run();
        }


        /// <summary>
        /// 创建主机生成器
        /// </summary>
        /// <typeparam name="TStartup"></typeparam>
        /// <param name="args"></param>
        /// <returns></returns>
        public IHostBuilder CreateBuilder<TStartup>(string[] args) where TStartup : H_Startup
        {
            var config = new ConfigurationBuilder()
                            .SetBasePath(AppContext.BaseDirectory)
                            .AddJsonFile("appsettings.json", false)
#if DEBUG
                            .AddJsonFile("appsettings.Development.json", false)
#endif
                            .Build();


            var appSettings = new AppSettingsInfo();
            config.GetSection("AppSettingsInfo").Bind(appSettings);

            return Host.CreateDefaultBuilder(args)
                       .UseServiceProviderFactory(new AutofacServiceProviderFactory(builder =>
                       {

                           var diAssemblies = appSettings.DiAssemblyNames.Select(name => Assembly.Load(name)).ToArray();

                           builder.RegisterAssemblyTypes(diAssemblies)
                                   .Where(m => typeof(ITransientDependency).IsAssignableFrom(m) && m != typeof(ITransientDependency)) //直接或间接实现了ITransientDependency
                                   .AsImplementedInterfaces().InstancePerDependency().PropertiesAutowired();

                           var controllerAssemblies = appSettings.ControllerAssemblyNames.Select(name => Assembly.Load(name));

                           var types = controllerAssemblies.SelectMany(a => a.GetExportedTypes()).Where(type => typeof(ControllerBase).IsAssignableFrom(type)).ToArray();
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
                               logging.AddNLog($"nlog.{hostingContext.HostingEnvironment.EnvironmentName}.config");
                           })
                           .UseNLog()
                           .UseUrls(appSettings.ServiceStartUrl)
                           .UseKestrel()
                           .UseStartup<TStartup>();
                       });
        }
    }
}
