using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Hao.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace HaoHaoPlay.ApiHost
{
    public class Startup : H_Startup
    {
        public Startup(IHostEnvironment env, IConfiguration cfg) : base(env, cfg, new DirectoryInfo(Directory.GetCurrentDirectory()))
        {
        }

        /// <summary>
        ///  用于配置依赖注入以在运行时根据依赖关系创建对象
        /// </summary>
        /// <param name="services"></param>
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
        }

        /// <summary>
        /// 用于配置中间件（middleware）以构建请求处理流水线
        /// </summary>
        /// <param name="app"></param>
        public override void Configure(IApplicationBuilder app)
        {
            base.Configure(app);
        }
    }
}
