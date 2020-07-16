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
    public class H_HostBuilder<TConfig> where TConfig : H_AppSettingsConfig, new()
    {
        /// <summary>
        /// 启动
        /// </summary>
        /// <typeparam name="TStartup"></typeparam>
        /// <param name="args"></param>
        public void Run<TStartup>(string[] args) where TStartup : H_Startup<TConfig>
        {

            var configBuilder = new ConfigurationBuilder();

            InitBuild(configBuilder);

            var configRoot = configBuilder.Build();

            var appSettings = new TConfig();
            configRoot.Bind(appSettings);

            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder =>
                {
                    InitBuild(builder);
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory(builder =>
                {
                    var diAssemblies = appSettings.DiAssemblyNames.Select(name => Assembly.Load(name)).ToArray();

                    builder.RegisterAssemblyTypes(diAssemblies).Where(m => typeof(ITransientDependency).IsAssignableFrom(m) && m != typeof(ITransientDependency)) //直接或间接实现了ITransientDependency
                        .AsImplementedInterfaces().InstancePerDependency().PropertiesAutowired();

                    builder.RegisterAssemblyTypes(diAssemblies).Where(m => typeof(ISingletonDependency).IsAssignableFrom(m) && m != typeof(ISingletonDependency))
                        .AsImplementedInterfaces().SingleInstance().PropertiesAutowired();

                    var controllerAssemblies = appSettings.ControllerAssemblyNames.Select(name => Assembly.Load(name));

                    var controllerTypes = controllerAssemblies.SelectMany(a => a.GetExportedTypes()).Where(type => typeof(ControllerBase).IsAssignableFrom(type)).ToArray();

                    builder.RegisterTypes(controllerTypes).PropertiesAutowired();

                    //调用RegisterDynamicProxy扩展方法在Autofac中注册动态代理服务和动态代理配置 aop
                    //在一般情况下可以使用抽象的AbstractInterceptorAttribute自定义特性类，它实现IInterceptor接口。AspectCore默认实现了基于Attribute的拦截器配置
                    builder.RegisterDynamicProxy();
                }))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureLogging((hostingContext, logBuilder) =>
                    {
                        logBuilder.ClearProviders()
#if DEBUG
                                  .SetMinimumLevel(LogLevel.Information)
                                  .AddFilter("Microsoft.Hosting", LogLevel.Information)
                                  .AddFilter("Microsoft", LogLevel.Error)
                                  .AddFilter("System", LogLevel.Error) //过滤Error等级以下（不报括Error）的信息
                                  //.AddFilter("DotNetCore.CAP", LogLevel.Information)
                                  .AddConsole()
#endif                            
                                  .AddNLog($"nlog.{hostingContext.HostingEnvironment.EnvironmentName}.config");
                    })
                    .UseUrls(appSettings.ServiceStartUrl)
                    .UseKestrel()
                    .UseStartup<TStartup>();
                })
                .Build()
                .Run();
        }



        public static void InitBuild(IConfigurationBuilder builder)
        {
            var basePath = AppContext.BaseDirectory + "Config";

            builder.SetBasePath(basePath)
                   .AddJsonFile("appsettings.json", false, true) //optional:（Whether the file is optional）是否可选，意思是如果配置文件不存在的时候是否要抛异常。第三个参数 reloadOnChange  json文件更改后是否重新加载。
#if DEBUG
                   .AddJsonFile("appsettings.Development.json", false, true) //false，不可选， 文件不存在，则会报错
#endif
                   ;
        } 
    }
}
