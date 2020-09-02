using AspectCore.Extensions.Autofac;
using Autofac;
using Hao.Library;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Hao.Core.Extensions
{
    public abstract class H_Startup<TConfig> where TConfig : H_AppSettingsConfig, new()
    {
        private readonly IHostEnvironment _env;

        private readonly IConfiguration _cfg;

        private readonly TConfig _appSettings;

        protected H_Startup(IHostEnvironment env, IConfiguration cfg)
        {
            var parentDir = new DirectoryInfo(AppContext.BaseDirectory).Parent;
         
            //parentDir = currentDir?.Parent.Parent.Parent.Parent.Parent; 验证是null 而不是抛出异常
            if (parentDir == null) throw new Exception("项目安置路径有误，请检查");

            _env = env;
            _cfg = cfg;

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
        /// <param name="services"></param>
        public virtual void ConfigureServices(IServiceCollection services)
        {
            var filePathOption = _cfg.GetSection(nameof(H_AppSettingsConfig.FilePath));

            filePathOption.GetSection(nameof(H_AppSettingsConfig.FilePath.ExportExcelPath)).Value = _appSettings.FilePath.ExportExcelPath;
            filePathOption.GetSection(nameof(H_AppSettingsConfig.FilePath.ImportExcelPath)).Value = _appSettings.FilePath.ImportExcelPath;
            filePathOption.GetSection(nameof(H_AppSettingsConfig.FilePath.AvatarPath)).Value = _appSettings.FilePath.AvatarPath;

            services.Configure<TConfig>(_cfg); //绑定配置对象 子类 （新增配置信息）

            services.Configure<H_AppSettingsConfig>(_cfg); //绑定配置对象 基类

            services.ConfigureServices(_env, _appSettings);
        }

        /// <summary>
        /// autofac实现ioc，aop
        /// </summary>
        /// <param name="builder"></param>
        public virtual void ConfigureContainer(ContainerBuilder builder)
        {
            var diAssemblies = _appSettings.DiAssemblyNames.Select(name => Assembly.Load(name)).ToArray();

            builder.RegisterAssemblyTypes(diAssemblies).Where(m => typeof(ITransientDependency).IsAssignableFrom(m) && m != typeof(ITransientDependency)) //直接或间接实现了ITransientDependency
                .AsImplementedInterfaces().InstancePerDependency().PropertiesAutowired();

            builder.RegisterAssemblyTypes(diAssemblies).Where(m => typeof(ISingletonDependency).IsAssignableFrom(m) && m != typeof(ISingletonDependency))
                .AsImplementedInterfaces().SingleInstance().PropertiesAutowired();

            var controllerAssemblies = _appSettings.ControllerAssemblyNames.Select(name => Assembly.Load(name));

            var controllerTypes = controllerAssemblies.SelectMany(a => a.GetExportedTypes()).Where(type => typeof(ControllerBase).IsAssignableFrom(type)).ToArray();

            builder.RegisterTypes(controllerTypes).PropertiesAutowired();

            //调用RegisterDynamicProxy扩展方法在Autofac中注册动态代理服务和动态代理配置 aop
            //在一般情况下可以使用抽象的AbstractInterceptorAttribute自定义特性类，它实现IInterceptor接口。AspectCore默认实现了基于Attribute的拦截器配置
            builder.RegisterDynamicProxy();
        }

        /// <summary>
        /// 用于配置中间件，以构建请求处理流水线
        /// </summary>
        /// <param name="app"></param>
        public virtual void Configure(IApplicationBuilder app)
        {
            //执行顺序 ConfigureContainer - Configure
            app.Configure(_env, _appSettings);
        }


        /// <summary>
        /// 检查配置
        /// </summary>
        /// <param name="config"></param>
        public virtual void CheckAppSettings(H_AppSettingsConfig config)
        {
            var message = "配置异常：";

            if (string.IsNullOrWhiteSpace(config.RedisPrefix.Login)) throw new Exception($"{message}RedisPrefix.Login");

            if (string.IsNullOrWhiteSpace(config.RedisPrefix.DistributedLock)) throw new Exception($"{message}RedisPrefix.DistributedLock");
        }
    }
}
