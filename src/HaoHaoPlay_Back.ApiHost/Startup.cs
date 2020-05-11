using Hao.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace HaoHaoPlay.ApiHost
{
    public class Startup : H_Startup
    {
        public Startup(IHostEnvironment env, IConfiguration cfg) : base(env, cfg, new DirectoryInfo(Directory.GetCurrentDirectory()))
        {
        }
    }
}
