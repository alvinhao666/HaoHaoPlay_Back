using Hao.Library;
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

        private readonly DirectoryInfo _parentDir = new DirectoryInfo(AppContext.BaseDirectory).Parent;

        protected H_Startup(IHostEnvironment env, IConfiguration cfg)
        {
            if (_parentDir == null) throw new Exception("项目安置路径有误，请检查");

            _env = env;
            _cfg = cfg;

            _appSettings = new AppSettingsInfo();
            cfg.Bind("AppSettingsInfo", _appSettings);
   
            _appSettings.FilePath.ExportExcelPath = Path.Combine(_parentDir.FullName, _appSettings.FilePath.ExportExcelPath);
            _appSettings.FilePath.ImportExcelPath = Path.Combine(_parentDir.FullName, _appSettings.FilePath.ImportExcelPath);
            _appSettings.FilePath.AvatarPath = Path.Combine(_parentDir.FullName, _appSettings.FilePath.AvatarPath);
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            var appSettingsOption = _cfg.GetSection(nameof(AppSettingsInfo));
            services.Configure<AppSettingsInfo>(appSettingsOption);

            services.AddWebHost(_env, _cfg, _appSettings);
        }

        public virtual void Configure(IApplicationBuilder app)
        {
            app.UseWebHost(_env, _appSettings);
        }
    }
}
