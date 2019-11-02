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
            CreateWebHostBuilder(args).Build().Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseUrls("http://*:8000")
            .UseKestrel()
            .UseStartup<Startup>()
            .ConfigureLogging((hostingContext,logging) =>
            {
                logging.AddNLog($"NLog.{hostingContext.HostingEnvironment.EnvironmentName}.config");
            })
            .UseNLog();
    }
}
