using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
using Hao.DbContext;
using FluentValidation.AspNetCore;
using FluentValidation;
using Hao.AppService.ViewModel;
using Microsoft.Extensions.FileProviders;
using Hao.FileHelper;
using Newtonsoft.Json.Serialization;
using CSRedis;
using Microsoft.Extensions.Caching.Redis;

namespace haohaoplay.Web.Host
{
    public class Startup
    {
        private const string SecretKey = "U8p6i6EQZg9sfxlN";

        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        public IConfigurationRoot Configuration { get; }

        public IContainer ApplicationContainer { get; private set; }


        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false) // 在运行时修改强类型配置，无需设置reloadOnChange = true,默认就为true,只需要使用IOptionsSnapshot接口,IOptions<> 生命周期为Singleton,IOptionsSnapshot<> 生命周期为Scope
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)  //没有的话 默认读取appsettings.json
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

#if DEBUG
            services.AddSwaggerGen(c =>
            {
                //配置第一个Doc
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "haohaoplay接口文档",
                    Description = "haohaoplay Restful Api 接口说明",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "rongguohao",
                        Email = "843468011@qq.com"
                    }
                });
                //c.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "haohaoplay.Web.Host.xml"));
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            //跨域
            var corsUrls = Configuration["Cors"].Split(',');
            services.AddCors(options =>
            options.AddPolicy("AllowAllOrigin",
            p => p.WithOrigins(corsUrls).AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin().AllowCredentials()));
#endif

            services.AddOptions();
            services.AddSingleton(Configuration);


            // Validators

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UserVMInValidator>());


            #region Session 获取用户
            services.AddSession();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //即使service使用了单例模式，但是在多线程的情况下，HttpContextAccessor不会出现线程同步问题。

            services.AddScoped<ICurrentUser, CurrentUser>();
            #endregion


            
            #region Jwt
            var jwtSection = Configuration.GetSection(nameof(JwtOptions));

            services.Configure<JwtOptions>(options =>
            {
                options.Audience = jwtSection[nameof(JwtOptions.Audience)];
                options.Issuer = jwtSection[nameof(JwtOptions.Issuer)];
                options.SigningKey = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            //services.AddAuthorization();

            //jwt验证：
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,//是否验证Issuer
                        ValidateAudience = true,//是否验证Audience
                        ValidateIssuerSigningKey = true,//是否验证SecurityKey
                        ValidAudience = jwtSection[nameof(JwtOptions.Audience)],//Audience
                        ValidIssuer = jwtSection[nameof(JwtOptions.Issuer)],//Issuer，这两项和前面签发jwt的设置一致
                        IssuerSigningKey = _signingKey,//拿到SecurityKey
                        ValidateLifetime = true,//是否验证失效时间
                        RequireExpirationTime = true,
                        ClockSkew = TimeSpan.Zero,
                    };
                    options.Events = new JwtBearerOverrideEvents();
                });
            #endregion

            #region 模型验证
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var error = context.ModelState
                        .Values
                        .SelectMany(x => x.Errors
                                    .Select(p => p.ErrorMessage))
                        .FirstOrDefault();

                    var result = new BaseResponse()
                    {
                        Success = false,
                        ErrorCode = nameof(ErrorCode.E100010).GetCode(),
                        ErrorMsg = error
                    };

                    return new ObjectResult(result);
                };
            });

            #endregion


            /**********数据ORM*******/
            services.AddSqlSugarClient(config =>
            {
                config.ConnectionString = Configuration.GetConnectionString("MySqlConnection");
                config.DbType = DbType.MySql;
                config.IsAutoCloseConnection = true;
                config.InitKeyType = InitKeyType.Attribute;//如果不是SA等高权限数据库的账号,需要从实体读取主键或者自增列 InitKeyType要设成Attribute
                config.ConfigureExternalServices = new ConfigureExternalServices()
                {
                    DataInfoCacheService = new HRedisCache() //RedisCache是继承ICacheService自已实现的一个类
                };

                //config.SlaveConnectionConfigs = new List<SlaveConnectionConfig>() { //= 如果配置了 SlaveConnectionConfigs那就是主从模式,所有的写入删除更新都走主库，查询走从库，事务内都走主库，HitRate表示权重 值越大执行的次数越高，如果想停掉哪个连接可以把HitRate设为0 
                //     new SlaveConnectionConfig() { HitRate=10, ConnectionString=Config.ConnectionString2 },
                //     new SlaveConnectionConfig() { HitRate=30, ConnectionString=Config.ConnectionString3 }
            });

            /**********AutoMapper*******/
            services.AddAutoMapper(cfg => {
                AutoMapperInitApi.InitMap(cfg);
                AutoMapperInitService.InitMap(cfg);
            });

            #region Redis
            //services.AddDistributedRedisCache(c =>
            //{
            //    c.Configuration = Configuration["Redis"];
            //    c.InstanceName = "HaoHaoPlayInstance";
            //});

            var csredis = new CSRedisClient("127.0.0.1:6379,abortConnect=false,connectRetry=3,connectTimeout=3000,defaultDatabase=1,syncTimeout=3000,version=3.2.1,responseTimeout=3000");

            //初始化 RedisHelper
            RedisHelper.Initialization(csredis);

            services.AddSingleton(new CSRedisCache(RedisHelper.Instance));
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

            #region 性能 压缩
            services.AddResponseCompression();
            #endregion


            services.AddHttpClient();

            //全局Filter json大小写
            services.AddMvc(x =>
            {
                x.Filters.Add(typeof(GlobalResultFilter));

            }).AddJsonOptions(op => {
                op.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss"; //时间序列化格式
                op.SerializerSettings.ContractResolver = new DefaultContractResolver();
            })
              .AddWebApiConventions();//处理HttpResponseMessage类型返回值的问题


            /**********依赖注入*******/
            // .net core 注入
            //services.AddScoped<IUserService, UserService>();
            //services.AddScoped<IUserRepository, UserRepository>();

            //注入 IOC AutoFac容器
            var builder = new ContainerBuilder();//实例化 AutoFac  容器   

            builder.RegisterAssemblyTypes(
                    Assembly.Load("Hao.AppService"),
                    Assembly.Load("Hao.Repository"))
                   .Where(t => t.Name.EndsWith("AppService") || t.Name.EndsWith("Repository"))
                   .Where(m => typeof(ITransientDependency).IsAssignableFrom(m) && m != typeof(ITransientDependency))
                   .AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.Populate(services);
            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);//第三方IOC接管 core内置DI容器
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

#if DEBUG

            //配置Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "haohaoplayApi");
                c.InjectStylesheet("/css/swagger_ui.css");
            });

            //使用跨域
            app.UseCors("AllowAllOrigin");
            //异常处理
            app.UseGlobalExceptionHandler(LogManager.GetCurrentClassLogger(), true, Configuration["Cors"].Split(',')[0]);
#else
            //异常处理
            app.UseGlobalExceptionHandler(LogManager.GetCurrentClassLogger());
#endif

            //文件访问权限
            app.UseWhen(a => a.Request.Path.Value.Contains("ExportExcel") || a.Request.Path.Value.Contains("ExportWord"), b => b.UseMiddleware<AuthorizeStaticFilesMiddleware>());

            app.UseStaticFiles();//使用默认文件夹wwwroot
            //导出excel路径
            var exportExcelPath = Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.FullName, "ExportFile/Excel");
            if (!HFile.IsExistDirectory(exportExcelPath))
                HFile.CreateDirectory(exportExcelPath);
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(exportExcelPath),
                RequestPath = "/ExportExcel",
            });
            //导出word路径
            var exportWordPath = Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.FullName, "ExportFile/Word");
            if (!HFile.IsExistDirectory(exportWordPath))
                HFile.CreateDirectory(exportWordPath);
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(exportWordPath),
                RequestPath = "/ExportWord",
            });


            //JWT
            app.UseAuthentication();

            app.UseMiddleware<JwtAuthorizationFilter>();

            loggerFactory.AddNLog();//添加NLog
            env.ConfigureNLog($"NLog.{env.EnvironmentName}.config");//读取Nlog配置文件

            app.UseSession();
            //app.UseHttpsRedirection();
            app.UseMvc();

            //nginx 获取ip
            app.UseForwardedHeaders(new ForwardedHeadersOptions()
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            //性能压缩
            app.UseResponseCompression();


        }
    }

    /// <summary>
	/// 重写JWT触发函数
	/// </summary>
	public class JwtBearerOverrideEvents : JwtBearerEvents
    {
        /// <summary>
        /// 没有JwtToken时触发,
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task Challenge(JwtBearerChallengeContext context)
        {
            context.Response.Clear();
            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json";
            BaseResponse response = new BaseResponse()
            {
                Success = false,
                ErrorCode = nameof(ErrorCode.E100001).GetCode(),
                ErrorMsg = ErrorCode.E100001
            };
            context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            context.HandleResponse();
            return base.Challenge(context);
        }
    }
}
