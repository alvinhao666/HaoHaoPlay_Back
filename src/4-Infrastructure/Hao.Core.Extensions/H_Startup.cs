using Hao.Library;
using Hao.RunTimeException;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace Hao.Core.Extensions
{
    public abstract class H_Startup
    {
        private readonly IHostEnvironment _env;

        private readonly IConfiguration _cfg;

        private readonly AppSettingsInfo _appSettings;

        protected H_Startup(IHostEnvironment env, IConfiguration cfg, DirectoryInfo currentDir)
        {
            var parentDir = currentDir?.Parent;
            //parentDir = currentDir?.Parent.Parent.Parent.Parent.Parent; //验证是null 而不是抛出异常
            if (parentDir == null) throw new Exception("项目安置路径有误，请检查");

            _env = env;
            _cfg = cfg;

            _appSettings = new AppSettingsInfo();
            cfg.Bind("AppSettingsInfo", _appSettings);
   
            _appSettings.FilePath.ExportExcelPath = Path.Combine(parentDir.FullName, _appSettings.FilePath.ExportExcelPath);
            _appSettings.FilePath.ImportExcelPath = Path.Combine(parentDir.FullName, _appSettings.FilePath.ImportExcelPath);
            _appSettings.FilePath.AvatarPath = Path.Combine(parentDir.FullName, _appSettings.FilePath.AvatarPath);
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            var appSettingsOption = _cfg.GetSection(nameof(AppSettingsInfo));

            var filePathOption = appSettingsOption.GetSection(nameof(AppSettingsInfo.FilePath));

            filePathOption.GetSection(nameof(AppSettingsInfo.FilePath.ExportExcelPath)).Value = _appSettings.FilePath.ExportExcelPath;
            filePathOption.GetSection(nameof(AppSettingsInfo.FilePath.ImportExcelPath)).Value = _appSettings.FilePath.ImportExcelPath;
            filePathOption.GetSection(nameof(AppSettingsInfo.FilePath.AvatarPath)).Value = _appSettings.FilePath.AvatarPath;

            services.Configure<AppSettingsInfo>(appSettingsOption);

            services.AddWebHost(_env, _cfg, _appSettings);
        }

        public virtual void Configure(IApplicationBuilder app)
        {
            app.UseWebHost(_env, _appSettings);
        }
    }
}
