using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using AspectCore.Extensions.Hosting;
using NLog.Web;
using System.IO;

namespace Hao.Core.Extensions
{
    public class H_HostBuilder
    {
        /// <summary>
        /// 启动
        /// </summary>
        /// <typeparam name="TStartup"></typeparam>
        /// <param name="args"></param>
        public void Run<TStartup>(string[] args) where TStartup : class
        {
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(InitBuild)
                .UseServiceContext()  //aspectcore di容器 [fromservicecontext] 属性注入
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
                                  .AddNLog($"ConfigFile/nlog.{hostingContext.HostingEnvironment.EnvironmentName}.config");
                    })
                    .UseStartup<TStartup>();
                })
                .Build()
                .Run();
        }



        private void InitBuild(IConfigurationBuilder builder)
        {
            var basePath = Path.Combine(AppContext.BaseDirectory, "ConfigFile");

            builder.SetBasePath(basePath)
                   .AddJsonFile("appsettings.json", false, true) //optional:（Whether the file is optional）是否可选，意思是如果配置文件不存在的时候是否要抛异常。第三个参数 reloadOnChange  json文件更改后是否重新加载。
#if DEBUG
                   .AddJsonFile("appsettings.Development.json", false, true) //false，不可选， 文件不存在，则会报错
#endif
                   ;
        } 
    }
}
