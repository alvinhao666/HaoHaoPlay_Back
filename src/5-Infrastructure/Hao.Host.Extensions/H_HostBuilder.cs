using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using AspectCore.Extensions.Hosting;
using System.IO;
using Serilog;

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
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(InitBuild)
                .UseServiceContext()
                .UseSerilog((context, configure) =>
                {
                    configure.ReadFrom.Configuration(context.Configuration);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<TStartup>();
                })
                .Build();

            ServiceLocator.SetServiceProvider(host.Services);

            host.Run();
        }



        private void InitBuild(IConfigurationBuilder builder)
        {
            var basePath = Path.Combine(AppContext.BaseDirectory, "config");

            builder.SetBasePath(basePath)
                   .AddJsonFile("appsettings.json", false, true) //optional:（Whether the file is optional）是否可选，意思是如果配置文件不存在的时候是否要抛异常。第三个参数 reloadOnChange  json文件更改后是否重新加载。
#if DEBUG
                   .AddJsonFile("appsettings.Development.json", false, true) //false，不可选， 文件不存在，则会报错
#endif
                   ;
        } 
    }
}
