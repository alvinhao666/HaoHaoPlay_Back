using Hao.Core.Extensions;

namespace HaoHaoPlay.ApiHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new H_HostBuilder().Run<Startup>(args);
        }
    }
}