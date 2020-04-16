using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Hao.Core;
using Hao.Library;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.FileProviders;
using CSRedis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using AutoMapper;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Hao.File;
using Microsoft.OpenApi.Models;
using System.Text.Encodings.Web;
using Hao.Event;
using Hao.Snowflake;
using Hao.Json;
using Hao.Core.Extensions;
using Hao.Utility;
using Hao.AppService;

namespace HaoHaoPlay.ApiHost
{
    public class Startup
    {
        private readonly DirectoryInfo _parentDir = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent;

        private readonly FilePathInfo _pathInfo;

        public IConfiguration Config { get; }

        public Startup(IConfiguration configuration)
        {
            Config = configuration;
            if (_parentDir == null) throw new Exception("项目安置路径有误，请检查");
            _pathInfo = new FilePathInfo
            {
                ExportExcelPath = Path.Combine(_parentDir.FullName, "ExportFile/Excel"),
                ImportExcelPath = Path.Combine(_parentDir.FullName, "ImportFile/Excel"),
                AvatarPath = Path.Combine(_parentDir.FullName,"AvatarFile")
            };
        }

        public void ConfigureServices(IServiceCollection services)
        {
            #region DeBug
#if DEBUG
            services.AddSwaggerGen(c =>
            {
                //配置第一个Doc
                c.SwaggerDoc("haohaoplay_back", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "接口文档",
                    Description = "接口说明"
                });
                //c.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HaoHaoPlay.WebHost.xml"));
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Hao.WebApi.xml"));
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Hao.AppService.xml"));
            });
#endif
            #endregion


            var appSettings = new AppSettingsInfo();
            Config.Bind("AppSettingsInfo", appSettings);
            var appSettingsOption = Config.GetSection(nameof(AppSettingsInfo));
            services.Configure<AppSettingsInfo>(appSettingsOption);

            #region 单例注入
            var worker = new IdWorker(appSettings.SnowflakeIdOptions.WorkerId, appSettings.SnowflakeIdOptions.DataCenterId);
            services.AddSingleton(worker);

            services.AddSingleton(_pathInfo);
            #endregion


            #region Jwt

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,//是否验证Issuer
                    ValidateAudience = true,//是否验证Audience
                    ValidateIssuerSigningKey = true,//是否验证SecurityKey
                    ValidAudience = appSettings.JwtOptions.Audience,//Audience
                    ValidIssuer = appSettings.JwtOptions.Issuer,//Issuer，这两项和前面签发jwt的设置一致
                    IssuerSigningKey = appSettings.JwtOptions.SecurityKey,//拿到SecurityKey
                    ValidateLifetime = true,//是否验证失效时间  当设置exp和nbf时有效 同时启用ClockSkew 
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero, // ClockSkew 属性，默认是5分钟缓冲。
                };
                options.Events = new HJwtBearerEvents();
            });
            #endregion


            #region Redis

            var csRedis = new CSRedisClient(appSettings.ConnectionStrings.RedisConnection);

            //初始化 RedisHelper
            RedisHelper.Initialization(csRedis);

            //利用分布式缓存
            //现在,ASP.NET Core引入了IDistributedCache分布式缓存接口，它是一个相当基本的分布式缓存标准API，可以让您对它进行编程，然后无缝地插入第三方分布式缓存
            //DistributedCache将拷贝缓存的文件到Slave节点
            services.AddSingleton<IDistributedCache>(new CSRedisCache(RedisHelper.Instance));

            #endregion


            //ORM
            services.AddPostgreSQLService(appSettings.ConnectionStrings.PostgreSqlConnection);

            //CAP
            services.AddCapService(new HCapConfig() {
                PostgreSqlConnection = appSettings.ConnectionStrings.PostgreSqlConnection,
                HostName=appSettings.RabbitMQ.HostName,
                VirtualHost=appSettings.RabbitMQ.VirtualHost,
                Port=appSettings.RabbitMQ.Port,
                UserName=appSettings.RabbitMQ.UserName,
                Password=appSettings.RabbitMQ.Password
            });


            //替换控制器所有者,详见有道笔记,放AddMvc前面 controller属性注入
            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            services.AddControllers(x =>
            {
                x.Filters.Add(typeof(HResultFilter));
            })
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginValidator>())
            .AddJsonOptions(o =>
            {
                //不加这个 接口接收参数 string类型的时间 转换 datetime类型报错 system.text.json不支持隐式转化
                //Newtonsoft.Json 等默认支持隐式转换, 不一定是个合理的方式
                o.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                o.JsonSerializerOptions.PropertyNamingPolicy = null;
                o.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverter());
                o.JsonSerializerOptions.Converters.Add(new LongJsonConvert());
            }); //.AddWebApiConventions()//处理HttpResponseMessage类型返回值的问题


            //模型验证 ApiBehaviorOptions 的统一模型验证配置一定要放到(.AddMvc)后面
            services.AddCheckViewModelService();

            //Http
            services.AddHttpClient();

            //数据保护
            services.AddDataProtection();

            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<IHttpHelper, HttpHelper>();
            services.AutoDependency(typeof(ILoginEventHandler));


      
            #region AutoMapper
            services.AddSingleton<IMapper>(new Mapper(new MapperConfiguration(cfg =>
            {
                Hao.WebApi.MapperInit.Map(cfg);
                Hao.AppService.MapperInit.Map(cfg);
            })));
            #endregion
            
            // services.BuildServiceContextProvider();
        }


        public void Configure(IApplicationBuilder app)
        {
            #region Debug
#if DEBUG
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("haohaoplay_back/swagger.json", "api");
                c.InjectStylesheet("/css/swagger_ui.css");
            });

            app.UseCors(x => x.AllowCredentials().AllowAnyMethod().AllowAnyHeader().WithOrigins(new string[] { "http://localhost:4200" }));
#endif
            #endregion

            app.UseExceptionMiddleware();

            #region 文件
            //文件访问权限
            app.UseWhen(a => a.Request.Path.Value.Contains("ExportExcel") || a.Request.Path.Value.Contains("template"), b => b.UseMiddleware<StaticFileMiddleware>());
            //使用默认文件夹wwwroot
            app.UseStaticFiles();
            //导出excel路径
            var exportExcelPath = Path.Combine(_parentDir.FullName, "ExportFile/Excel");
            HFile.CreateDirectory(exportExcelPath);

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(exportExcelPath),
                RequestPath = "/ExportExcel",
                ContentTypeProvider = new FileExtensionContentTypeProvider(new Dictionary<string, string>
                    {
                        { ".xlsx","application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"}
                    })
            });

            //头像路径
            var avatarPath = Path.Combine(_parentDir.FullName, "AvatarFile");
            HFile.CreateDirectory(avatarPath);
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(avatarPath),
                RequestPath = "/AvatarFile"
            });
            #endregion


            #region  Nginx 获取ip
            app.UseForwardedHeaders(new ForwardedHeadersOptions()
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            #endregion

            app.UseRouting();

            #region 权限 .net core3.0需要放UseRouting后面
            app.UseAuthentication();
            app.UseAuthorization();
            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
