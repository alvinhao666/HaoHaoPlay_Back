using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace HaoHaoPlay.Host
{
    public class Program
    {
        /// <summary>
        /// Main方法入口
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Information);
                    logging.AddConsole();
                    logging.AddNLog($"NLog.{hostingContext.HostingEnvironment.EnvironmentName}.config");
                })
                .UseNLog()
                .UseUrls("http://*:8000")
                .UseKestrel()
                .UseStartup<Startup>()
                .Build()
                .Run();

            //.ConfigureAppConfiguration((hostingContext, config) =>
            //{
            //    var env = hostingContext.HostingEnvironment;
            //    config.SetBasePath(env.ContentRootPath);
            //    config.AddJsonFile("appsettings.json", optional: true,reloadOnChange:true);
            //    config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true,reloadOnChange:true);//没有的话 默认读取appsettings.json
            //    config.AddEnvironmentVariables();
            //}) //WebHost.CreateDefaultBuilder(args)内部已经配置
        }
    }
}