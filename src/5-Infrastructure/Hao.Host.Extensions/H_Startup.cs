using Hao.Library;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;


namespace Hao.Core.Extensions
{
    public abstract class H_Startup<TConfig> where TConfig : H_AppSettings, new()
    {
        protected readonly IHostEnvironment Env;

        protected readonly IConfiguration Configuration;

        private readonly TConfig _appSettings;

        protected H_Startup(IHostEnvironment env, IConfiguration cfg)
        {
            var parentDir = new DirectoryInfo(AppContext.BaseDirectory).Parent;

            //parentDir = currentDir?.Parent.Parent.Parent.Parent.Parent; 验证是null 而不是抛出异常
            if (parentDir == null) throw new Exception("项目安置路径有误，请检查");

            Env = env;
            Configuration = cfg;

            _appSettings = new TConfig();
            cfg.Bind(_appSettings);

            //检查配置项
            CheckAppSettings(_appSettings);

            _appSettings.FilePath.ExportExcelPath = Path.Combine(parentDir.FullName, _appSettings.FilePath.ExportExcelPath);
            _appSettings.FilePath.ImportExcelPath = Path.Combine(parentDir.FullName, _appSettings.FilePath.ImportExcelPath);
            _appSettings.FilePath.AvatarPath = Path.Combine(parentDir.FullName, _appSettings.FilePath.AvatarPath);
        }

        /// <summary>
        /// 用于配置依赖注入以在运行时根据依赖关系创建对象
        /// </summary>
        public virtual void ConfigureServices(IServiceCollection services)
        {
            var filePathOption = Configuration.GetSection(nameof(H_AppSettings.FilePath));

            filePathOption.GetSection(nameof(H_AppSettings.FilePath.ExportExcelPath)).Value = _appSettings.FilePath.ExportExcelPath;
            filePathOption.GetSection(nameof(H_AppSettings.FilePath.ImportExcelPath)).Value = _appSettings.FilePath.ImportExcelPath;
            filePathOption.GetSection(nameof(H_AppSettings.FilePath.AvatarPath)).Value = _appSettings.FilePath.AvatarPath;

            services.Configure<TConfig>(Configuration); //绑定配置对象 子类 （新增配置信息）

            services.Configure<H_AppSettings>(Configuration); //绑定配置对象 基类

            services.ConfigureServices(Env, _appSettings);
        }

        /// <summary>
        /// 用于配置中间件，以构建请求处理流水线
        /// </summary>
        public virtual void Configure(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            //执行顺序 ConfigureContainer - Configure
            app.Configure(Env, serviceProvider, _appSettings);

            ServiceLocator.SetServiceProvider(serviceProvider);
        }


        /// <summary>
        /// 检查配置
        /// </summary>
        public virtual void CheckAppSettings(H_AppSettings config)
        {
            var message = "配置异常：";

            if (string.IsNullOrWhiteSpace(config.RedisPrefix.Login)) throw new Exception($"{message}RedisPrefix.Login");

            if (string.IsNullOrWhiteSpace(config.RedisPrefix.DistributedLock)) throw new Exception($"{message}RedisPrefix.DistributedLock");
        }
    }
}
