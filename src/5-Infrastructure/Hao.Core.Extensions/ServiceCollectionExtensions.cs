using DotNetCore.CAP;
using FluentValidation.AspNetCore;
using Hao.Library;
using Hao.Snowflake;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using AspectCore.Extensions.DependencyInjection;
using Mapster;
using FreeSql;
using FreeRedis;
using MapsterMapper;

namespace Hao.Core.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IHostEnvironment env, H_AppSettings appSettings)
        {
            #region DevEnvironment

            if (env.IsDevelopment())
            {
                services.AddSwaggerGen(c =>
                {
                    //配置第一个Doc
                    c.SwaggerDoc(appSettings.Swagger.Name, new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "接口文档"
                    });
                    
                    c.SchemaFilter<EnumSchemaFilter>();
                    c.SchemaFilter<SwaggerIgnoreFilter>();
                    c.OperationFilter<IgnorePropertyFilter>();
                    
                    foreach (var item in appSettings.Swagger.Xmls)
                    {
                        c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, item));
                    }
                });
            }

            #endregion


            #region HttpClient

            services.AddHttpClient();

            #endregion


            #region 数据保护

            services.AddDataProtection();

            #endregion


            #region Mapper
            
            TypeAdapterConfig.GlobalSettings.Scan(appSettings.MapperAssemblyNames.Select(Assembly.Load).ToArray());

            #endregion


            #region 批量注入

            services.AutoDependency(appSettings.DiAssemblyNames.Select(Assembly.Load));

            #endregion


            #region 缓存&数据库

            //redis
            RedisHelper.AddClient(new RedisClient(appSettings.ConnectionString.Redis));

            //雪花id
            services.AddSingleton(new IdWorker(appSettings.SnowflakeId.WorkerId, appSettings.SnowflakeId.DataCenterId));

            //orm
            services.AddOrmService(DataType.PostgreSQL, appSettings.ConnectionString.Master, appSettings.ConnectionString.Slave.Select(a => a.Connection).ToArray());
            #endregion


            #region 消息中间件
            //Note: The injection of services needs before of `services.AddCap()`
            services.Scan(a =>
            {
                a.FromAssemblies(appSettings.EventSubscribeAssemblyNames.Select(name => Assembly.Load(name)))
                 .AddClasses(x => x.AssignableTo(typeof(ICapSubscribe)))  //直接或间接实现了ICapSubscribe
                 .AsSelf()
                 .WithTransientLifetime();
            });

            //CAP
            services.AddCapService(new H_CapConfig()
            {
                PostgreSqlConnection = appSettings.RabbitMQ.PostgreSqlConnection,
                HostName = appSettings.RabbitMQ.HostName,
                VirtualHost = appSettings.RabbitMQ.VirtualHost,
                Port = appSettings.RabbitMQ.Port,
                UserName = appSettings.RabbitMQ.UserName,
                Password = appSettings.RabbitMQ.Password
            });
            #endregion


            #region MvcBuilder

            //替换控制器所有者,详见有道笔记,放AddMvc前面 controller属性注入
            //services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            services.AddControllers(x =>
            {
                x.ModelBinderProviders.Insert(0, new StringTrimModelBinderProvider()); //去除模型字符串首尾 空格 FromQuery 评估模型绑定器时，按顺序检查提供程序的集合。 使用第一个返回与输入模型匹配的联编程序的提供程序。 因此，将提供程序添加到集合的末尾可能会导致在自定义联编程序有可能之前调用内置模型联编程序 https://docs.microsoft.com/zh-cn/aspnet/core/mvc/advanced/custom-model-binding?view=aspnetcore-3.1
                x.Filters.Add<H_ExceptionFilter>();
                x.Filters.Add<H_ResultFilter>();
            })
            .AddControllersAsServices() //controller属性注入 .net core 3.1版本   //实现了两件事情 - 它将您应用程序中的所有控制器注册到 DI 容器（如果尚未注册），并将IControllerActivator注册为ServiceBasedControllerActivator
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblies(appSettings.ValidatorAssemblyNames.Select(Assembly.Load)))
            .AddJsonOptions(o =>
            {
                //FromBody的数据        
                o.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                o.JsonSerializerOptions.PropertyNamingPolicy = null; //o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase //开头字母小写 默认
                o.JsonSerializerOptions.Converters.Add(new StringJsonConvert()); //去除模型字符串首尾 空格
                o.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverter()); //不加这个 接口接收参数 string类型的时间 转换 datetime类型报错 system.text.json不支持隐式转化    //Newtonsoft.Json 等默认支持隐式转换, 不一定是个合理的方式
                o.JsonSerializerOptions.Converters.Add(new LongJsonConvert());
            }); //.AddWebApiConventions() 处理HttpResponseMessage类型返回值的问题

            #endregion


            #region 模型验证 

            //ApiBehaviorOptions 的统一模型验证配置一定要放到(.AddMvc)后面
            services.AddValidateModelService();

            #endregion


            #region Json Web Token

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
                    ValidAudience = appSettings.Jwt.Audience,//Audience
                    ValidIssuer = appSettings.Jwt.Issuer,//Issuer，这两项和前面签发jwt的设置一致
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.Jwt.SecretKey)),//拿到SecurityKey
                    ValidateLifetime = true,//是否验证失效时间  当设置exp和nbf时有效 同时启用ClockSkew 
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero // 失效偏离时间，可以设置失效后xx时间内有效
                };
                options.Events = new H_JwtBearerEvents();
            });
            #endregion


            #region AOP

            services.ConfigureDynamicProxy();

            #endregion


            return services;
        }
    }
}
