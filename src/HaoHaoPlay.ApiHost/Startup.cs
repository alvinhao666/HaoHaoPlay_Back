using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System.Reflection;
using Hao.Core;
using Hao.Library;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Hao.Response;
using Microsoft.AspNetCore.HttpOverrides;
using Hao.WebApi;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.FileProviders;
using CSRedis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using AutoMapper;
using Microsoft.AspNetCore.StaticFiles;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Hao.File;
using Microsoft.OpenApi.Models;
using System.Text.Encodings.Web;
using Hao.Event;
using Hao.Snowflake;
using Hao.Json;

namespace HaoHaoPlay.ApiHost
{
    public class Startup
    {
        private readonly DirectoryInfo _parentDir = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent;

        private readonly FilePathInfo pathInfo;

        public IConfiguration Config { get; }

        public Startup(IConfiguration configuration)
        {
            Config = configuration;
            if (_parentDir == null) throw new Exception("项目安置路径有误，请检查");
            pathInfo = new FilePathInfo();
            pathInfo.ExportExcelPath = Path.Combine(_parentDir.FullName, "ExportFile/Excel");
            pathInfo.ImportExcelPath = Path.Combine(_parentDir.FullName, "ImportFile/Excel");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var appsettings = new AppSettingsInfo();
            Config.Bind("AppSettingsInfo", appsettings);
            var appsettingsOptions = Config.GetSection(nameof(AppSettingsInfo));
            services.Configure<AppSettingsInfo>(appsettingsOptions);

            #region DeBug
#if DEBUG
            services.AddSwaggerGen(c =>
            {
                //配置第一个Doc
                c.SwaggerDoc("v1", new OpenApiInfo
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
            });
#endif
            #endregion


            #region 单例注入
            var worker = new IdWorker(appsettings.SnowflakeIdOptions.WorkerId, appsettings.SnowflakeIdOptions.DataCenterId);
            services.AddSingleton(worker);

            services.AddSingleton(pathInfo);
            #endregion


            #region Http
            services.AddHttpClient();
            #endregion


            #region 数据保护
            services.AddDataProtection();
            #endregion


            #region Jwt

            //jwt验证：
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
                    ValidAudience = appsettings.JwtOptions.Audience,//Audience
                    ValidIssuer = appsettings.JwtOptions.Issuer,//Issuer，这两项和前面签发jwt的设置一致
                    IssuerSigningKey = appsettings.JwtOptions.SecurityKey,//拿到SecurityKey
                    ValidateLifetime = true,//是否验证失效时间  当设置exp和nbf时有效 同时启用ClockSkew 
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero, // ClockSkew 属性，默认是5分钟缓冲。
                };
                options.Events = new HJwtBearerEvents();
            });
            #endregion


            #region Redis

            var csRedis = new CSRedisClient(appsettings.ConnectionStrings.RedisConnection);

            //初始化 RedisHelper
            RedisHelper.Initialization(csRedis);

            services.AddSingleton<IDistributedCache>(new CSRedisCache(RedisHelper.Instance)); //利用分布式缓存
                                                                                              //现在,ASP.NET Core引入了IDistributedCache分布式缓存接口，它是一个相当基本的分布式缓存标准API，可以让您对它进行编程，然后无缝地插入第三方分布式缓存
                                                                                              //DistributedCache将拷贝缓存的文件到Slave节点
            #endregion


            #region ORM

            services.AddPostgreSQLService(appsettings.ConnectionStrings.PostgreSqlConnection);

            #endregion


            #region CAP

            services.AddCap(x =>
            {
                x.UseDashboard();

                x.UsePostgreSql(cfg => { cfg.ConnectionString = appsettings.ConnectionStrings.PostgreSqlConnection; });

                x.UseRabbitMQ(cfg =>
                {
                    cfg.HostName = appsettings.RabbitMQ.HostName;
                    cfg.VirtualHost = appsettings.RabbitMQ.VirtualHost; // 相当于数据库 可以在rabbitmq管理后台里面进行添加
                    cfg.Port = appsettings.RabbitMQ.Port;
                    cfg.UserName = appsettings.RabbitMQ.UserName;
                    cfg.Password = appsettings.RabbitMQ.Password;
                });

                x.FailedRetryCount = 2; //失败重试机会
                x.FailedRetryInterval = 5;
                x.SucceedMessageExpiredAfter = 24 * 3600;
                // If you are using Kafka, you need to add the configuration：
                //x.UseKafka("localhost");
            });
            services.AutoDependency(typeof(ILoginEventHandler));
            #endregion


            #region Session 获取当前用户
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);//设置session的过期时间
            });

            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //即使service使用了单例模式，但是在多线程的情况下，HttpContextAccessor不会出现线程同步问题。// .net core 2.2不需要

            services.AddScoped<ICurrentUser, CurrentUser>();
            #endregion


            //替换控制器所有者,详见有道笔记,放AddMvc前面
            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            services.AddControllers(x =>
            {
                x.Filters.Add(typeof(HResultFilter));
            })
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginInValidator>()) //模型验证
            //.AddWebApiConventions()//处理HttpResponseMessage类型返回值的问题
            .AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                o.JsonSerializerOptions.PropertyNamingPolicy = null;
                o.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverter());
                //不加这个 接口接收参数 string类型的时间 转换 datetime类型报错 system.text.json不支持隐式转化
                //Newtonsoft.Json 等默认支持隐式转换, 不一定是个合理的方式
            });


            #region 模型验证 ApiBehaviorOptions 的统一模型验证配置一定要放到(.AddMvc)后面
            services.AddInvalidModel();
            #endregion


            #region AutoMapper
            services.AddSingleton<IMapper>(new Mapper(new MapperConfiguration(cfg =>
            {
                Hao.WebApi.MapperInit.Map(cfg);
                Hao.AppService.MapperInit.Map(cfg);
            })));
            #endregion
        }


        public void Configure(IApplicationBuilder app)
        {
            #region Debug
#if DEBUG
            //配置Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "api");
                c.InjectStylesheet("/css/swagger_ui.css");
            });

            app.UseCors(x => x.AllowCredentials().AllowAnyMethod().AllowAnyHeader().WithOrigins(new string[] { "http://localhost:4200" }));
#endif
            #endregion


            #region 获取当前用户
            app.UseSession();
            #endregion


            #region 异常处理
            app.UseGlobalExceptionHandler();
            #endregion


            #region 文件
            //文件访问权限
            app.UseWhen(a => a.Request.Path.Value.Contains("ExportExcel") || a.Request.Path.Value.Contains("template"), b => b.UseMiddleware<AuthorizeStaticFilesMiddleware>());
            app.UseStaticFiles();//使用默认文件夹wwwroot
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
