﻿using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Hao.Core;
using Hao.Core.Dependency;
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

                    builder.RegisterAssemblyTypes(
                        Assembly.Load("Hao.Repository"),
                        Assembly.Load("Hao.Core"))
                        .Where(m => typeof(ITransientDependency).IsAssignableFrom(m) && m != typeof(ITransientDependency))
                        .AsImplementedInterfaces().InstancePerLifetimeScope().PropertiesAutowired();

                    builder.RegisterAssemblyTypes(
                            Assembly.Load("Hao.AppService"))
                            .Where(m => typeof(ITransientDependency).IsAssignableFrom(m) && m != typeof(ITransientDependency))
                            .AsImplementedInterfaces().InstancePerLifetimeScope().PropertiesAutowired().EnableInterfaceInterceptors().InterceptedBy(typeof(HTransactionAop));
                    //一定要在你注入的服务后面加上EnableInterfaceInterceptors来开启你的拦截(aop)


                    var controllersTypesInAssembly = typeof(HController).Assembly.GetExportedTypes()
                                    .Where(type => typeof(Controller).IsAssignableFrom(type)).ToArray();
                    builder.RegisterTypes(controllersTypesInAssembly).PropertiesAutowired();
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