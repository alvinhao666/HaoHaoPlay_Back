using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace HaoHaoPlay.Host
{
    public class Program
    {

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
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
                }).UseServiceProviderFactory(new AutofacServiceProviderFactory());
    }
}