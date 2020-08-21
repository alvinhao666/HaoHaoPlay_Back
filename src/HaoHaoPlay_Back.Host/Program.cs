using Hao.Core.Extensions;

namespace HaoHaoPlay_Back.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new H_HostBuilder().Run<Startup<AppSettingsConfig>>(args);
        }
    }
}