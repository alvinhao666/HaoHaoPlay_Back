using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using SqlSugar;
using Autofac;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using Hao.Core.Dependency;
using Hao.Core;
using Microsoft.AspNetCore.Http;
using Hao.Library;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Hao.Core.Response;
using Newtonsoft.Json;
using Hao.Core.Filter;
using NLog.Extensions.Logging;
using NLog.Web;
using NLog;
using Microsoft.AspNetCore.HttpOverrides;
using Hao.WebApi;
using Hao.AppService;
using Hao.Event;
using Hao.SqlSugarExtensions;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Serialization;
using CSRedis;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Caching.Distributed;
using AutoMapper;
using Microsoft.AspNetCore.StaticFiles;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Snowflake.Core;
using Autofac.Extras.DynamicProxy;
using Hao.File;

namespace HaoHaoPlay.Host
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        private IContainer ApplicationContainer { get; set; }


        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) // 在运行时修改强类型配置，无需设置reloadOnChange = true,默认就为true,只需要使用IOptionsSnapshot接口,IOptions<> 生命周期为Singleton,IOptionsSnapshot<> 生命周期为Scope
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)  // 没有的话 默认读取appsettings.json
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            #region DeBug
#if DEBUG
            services.AddSwaggerGen(c =>
            {
                //配置第一个Doc
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "接口文档",
                    Description = "接口说明",
                    Contact = new Contact
                    {
                        Name = "rongguohao",
                        Email = "843468011@qq.com"
                    }
                });
                //c.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HaoHaoPlay.WebHost.xml"));
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
#endif
            #endregion

            #region Http
            services.AddHttpClient();
            #endregion

            #region Jwt
            var jwtSection = Configuration.GetSection(nameof(JwtOptions));

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSection[nameof(JwtOptions.SecretKey)]));

            services.Configure<JwtOptions>(options =>
            {
                options.Audience = jwtSection[nameof(JwtOptions.Audience)];
                options.Issuer = jwtSection[nameof(JwtOptions.Issuer)];
                options.SigningKey = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

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
                    ValidAudience = jwtSection[nameof(JwtOptions.Audience)],//Audience
                    ValidIssuer = jwtSection[nameof(JwtOptions.Issuer)],//Issuer，这两项和前面签发jwt的设置一致
                    IssuerSigningKey = signingKey,//拿到SecurityKey
                    ValidateLifetime = true,//是否验证失效时间  当设置exp和nbf时有效 同时启用ClockSkew 
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero, // ClockSkew 属性，默认是5分钟缓冲。
                };
                options.Events = new JwtBearerOverrideEvents();
            });
            #endregion

            #region Redis
            services.AddDistributedRedisCache(c =>
            {
                c.Configuration = Configuration["Redis"];
                c.InstanceName = "HaoHaoPlayInstance";
            });

            var csRedis = new CSRedisClient(Configuration.GetConnectionString("RedisConnection"));

            //初始化 RedisHelper
            RedisHelper.Initialization(csRedis);

            services.AddSingleton<IDistributedCache>(new CSRedisCache(RedisHelper.Instance)); //利用分布式缓存
                                                                                              //现在,ASP.NET Core引入了IDistributedCache分布式缓存接口，它是一个相当基本的分布式缓存标准API，可以让您对它进行编程，然后无缝地插入第三方分布式缓存
                                                                                              //DistributedCache将拷贝缓存的文件到Slave节点

            var redisPrefix = Configuration.GetSection(nameof(RedisPrefixOptions));
            services.Configure<RedisPrefixOptions>(redisPrefix);
            #endregion

            #region ORM
            services.AddSqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = Configuration.GetConnectionString("MySqlConnection"),
                DbType = DbType.MySql,
                IsAutoCloseConnection = true,//开启自动释放模式和EF原理一样 自动释放数据务，如果存在事务，在事务结束后释放
                InitKeyType = InitKeyType.SystemTable,  //如果不是SA等高权限数据库的账号,需要从实体读取主键或者自增列 InitKeyType要设成Attribute (不需要读取这些信息)
                ConfigureExternalServices = new ConfigureExternalServices()
                {
                    DataInfoCacheService = new SqlSugarRedisCache() //RedisCache是继承ICacheService自已实现的一个类
                }

                //config.SlaveConnectionConfigs = new List<SlaveConnectionConfig>() { //= 如果配置了 SlaveConnectionConfigs那就是主从模式,所有的写入删除更新都走主库，查询走从库，事务内都走主库，HitRate表示权重 值越大执行的次数越高，如果想停掉哪个连接可以把HitRate设为0 
                //     new SlaveConnectionConfig() { HitRate=10, ConnectionString=Config.ConnectionString2 },
                //     new SlaveConnectionConfig() { HitRate=30, ConnectionString=Config.ConnectionString3 }
            });


            #endregion

            #region CAP
            services.AddTransient<IPersonEventHandler, PersonEventHandler>();
            services.AddCap(x =>
            {

                x.UseMySql(cfg => { cfg.ConnectionString = Configuration.GetConnectionString("MySqlConnection"); });

                x.UseRabbitMQ(cfg =>
                {
                    cfg.HostName = Configuration["RabbitMQ:HostName"];
                    cfg.VirtualHost = Configuration["RabbitMQ:VirtualHost"];
                    cfg.Port = Convert.ToInt32(Configuration["RabbitMQ:Port"]);
                    cfg.UserName = Configuration["RabbitMQ:UserName"];
                    cfg.Password = Configuration["RabbitMQ:Password"];
                });

                x.FailedRetryCount = 2;
                x.FailedRetryInterval = 5;
                // If you are using Kafka, you need to add the configuration：
                //x.UseKafka("localhost");
            });
            #endregion

            #region 模型验证
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            #endregion

            #region Session 获取当前用户
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);//设置session的过期时间
            });

            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //即使service使用了单例模式，但是在多线程的情况下，HttpContextAccessor不会出现线程同步问题。// .net core 2.2不需要

            services.AddScoped<ICurrentUser, CurrentUser>();
            #endregion

            #region 单例注入
            var snowflake = Configuration.GetSection(nameof(SnowflakeIdOptions));
            var worker = new IdWorker(long.Parse(snowflake[nameof(SnowflakeIdOptions.WorkerId)]), long.Parse(snowflake[nameof(SnowflakeIdOptions.DataCenterId)]));
            services.AddSingleton(worker);

            var logger = LogManager.GetCurrentClassLogger();
            services.AddSingleton<NLog.ILogger>(logger);
            #endregion

            #region 数据保护
            services.AddDataProtection();
            #endregion

            #region 性能 压缩
            services.AddResponseCompression();
            #endregion


            #region AutoMapper
            services.AddSingleton<IMapper>(new Mapper(new MapperConfiguration(cfg =>
            {
                AutoMapperInitApi.InitMap(cfg);
                AutoMapperInitService.InitMap(cfg);
            })));
            #endregion


            //替换控制器所有者,详见有道笔记,放AddMvc前面
            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            services.AddMvc(x =>
            {
                x.Filters.Add(typeof(HValidationFilter));
                x.Filters.Add(typeof(HResultFilter));
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UserVMInValidator>()) //模型验证
            .AddJsonOptions(op =>
            {
                op.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss"; //时间序列化格式
                op.SerializerSettings.ContractResolver = new DefaultContractResolver(); //全局Filter json大小写
            }).AddWebApiConventions();//处理HttpResponseMessage类型返回值的问题



            #region Autofac
            var builder = new ContainerBuilder();//实例化 AutoFac  容器   

            builder.RegisterType<HTransactionAop>();

            builder.RegisterAssemblyTypes(
                Assembly.Load("Hao.Repository"),
                Assembly.Load("Hao.Core"))
               .Where(m => typeof(ITransientDependency).IsAssignableFrom(m) && m != typeof(ITransientDependency))
               .AsImplementedInterfaces().InstancePerLifetimeScope().PropertiesAutowired();

            builder.RegisterAssemblyTypes(
                    Assembly.Load("Hao.AppService"))
                   //.Where(t => t.Name.EndsWith("AppService") || t.Name.EndsWith("Repository"))
                   .Where(m => typeof(ITransientDependency).IsAssignableFrom(m) && m != typeof(ITransientDependency))
                   .AsImplementedInterfaces().InstancePerLifetimeScope().PropertiesAutowired().EnableInterfaceInterceptors().InterceptedBy(typeof(HTransactionAop));
            //一定要在你注入的服务后面加上EnableInterfaceInterceptors来开启你的拦截(aop)


            #region 属性注入
            var controllersTypesInAssembly = typeof(HController).Assembly.GetExportedTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type)).ToArray();
            builder.RegisterTypes(controllersTypesInAssembly).PropertiesAutowired();
            #endregion


            builder.Populate(services);
            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);//第三方IOC接管 core内置DI容器
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
#if DEBUG 
            //配置Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "haohaoplayApi");
                c.InjectStylesheet("/css/swagger_ui.css");
            });

            app.UseCors(x => x.AllowAnyHeader().AllowCredentials().AllowAnyMethod().AllowAnyOrigin());
#endif
            #region 日志
            loggerFactory.AddNLog();//添加NLog
            env.ConfigureNLog($"NLog.{env.EnvironmentName}.config");//读取Nlog配置文件
            #endregion

            #region 权限
            app.UseAuthentication();//[Authorize]
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
            var directoryInfo = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent;
            if (directoryInfo != null)
            {
                var exportExcelPath = Path.Combine(directoryInfo.FullName, "ExportFile/Excel");
                if (!HFile.IsExistDirectory(exportExcelPath))
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
            }

            #endregion

            #region 获取客户端ip
            // Nginx 获取ip
            app.UseForwardedHeaders(new ForwardedHeadersOptions()
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            #endregion

            #region 性能压缩
            app.UseResponseCompression();
            #endregion

            app.UseMvc();
        }
    }

    /// <summary>
	/// 重写JWT触发函数
	/// </summary>
	public class JwtBearerOverrideEvents : JwtBearerEvents
    {
        #region 暂不需要重写
        ///// <summary>
        ///// 接收时
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //public override Task MessageReceived(MessageReceivedContext context)
        //{
        //    context.Token = context.Request.Headers["Authorization"];
        //    return Task.CompletedTask;
        //}

        ///// <summary>
        ///// TokenValidated：在Token验证通过后调用。
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //public override Task TokenValidated(TokenValidatedContext context)
        //{

        //    return Task.CompletedTask;
        //}

        ///// <summary>
        ///// AuthenticationFailed: 认证失败时调用。
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>        
        //public override Task AuthenticationFailed(AuthenticationFailedContext context)
        //{
        //    return Task.CompletedTask;
        //}
        #endregion

        /// <summary>
        /// token过期 或者 没有token会出发此方法。 使用时一定要在 Controller上加[Authorize]
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task Challenge(JwtBearerChallengeContext context)
        {
            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status200OK;
            context.Response.ContentType = "application/json";
            var response = new BaseResponse()
            {
                Success = false,
                ErrorCode = nameof(ErrorInfo.E100001).GetErrorCode(),
                ErrorMsg = ErrorInfo.E100001
            };
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            context.HandleResponse();
        }
    }
}
