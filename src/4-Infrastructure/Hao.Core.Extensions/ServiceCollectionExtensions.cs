using AutoMapper;
using CSRedis;
using DotNetCore.CAP;
using FluentValidation.AspNetCore;
using Hao.Library;
using Hao.Snowflake;
using Hao.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
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

namespace Hao.Core.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IHostEnvironment env, H_AppSettingsConfig appSettings)
        {
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

                    foreach (var item in appSettings.Swagger.Xmls)
                    {
                        c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, item));
                    }
                });
            }


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
                    ValidAudience = appSettings.Jwt.Audience,//Audience
                    ValidIssuer = appSettings.Jwt.Issuer,//Issuer，这两项和前面签发jwt的设置一致
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.Jwt.SecretKey)),//拿到SecurityKey
                    ValidateLifetime = true,//是否验证失效时间  当设置exp和nbf时有效 同时启用ClockSkew 
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero, // ClockSkew 属性，默认是5分钟缓冲。
                };
                options.Events = new H_JwtBearerEvents();
            });
            #endregion


            #region Redis

            var csRedis = new CSRedisClient(appSettings.ConnectionString.Redis);

            //初始化 RedisHelper
            RedisHelper.Initialization(csRedis);

            //利用IDistributedCache接口
            services.AddSingleton<IDistributedCache>(new CSRedisCache(RedisHelper.Instance)); //nuget caching.csredis

            #endregion


            #region Orm

            services.AddPostgreSqlService(appSettings.ConnectionString.PostgreSql_Master, appSettings.ConnectionString.PostgreSql_Slave.ToDictionary(a => a.Connection, a => a.Weight));

            #endregion


            #region CAP
            //Note: The injection of services needs before of `services.AddCap()`
            services.Scan(a =>
            {
                a.FromAssemblies(appSettings.EventSubscribeAssemblyNames.Select(name => Assembly.Load(name)))
                 .AddClasses(x => x.AssignableTo(typeof(ICapSubscribe)))  //直接或间接实现了ICapSubscribe
                 .AsImplementedInterfaces()
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

            //替换控制器所有者,详见有道笔记,放AddMvc前面 controller属性注入
            //services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            services.AddControllers(x =>
            {
                x.ModelBinderProviders.Insert(0, new StringTrimModelBinderProvider()); //去除模型字符串首尾 空格 fromquery
                x.Filters.Add(typeof(H_ResultFilter));
            })
            .AddControllersAsServices() //controller属性注入 .net core 3.1版本   //实现了两件事情 - 它将您应用程序中的所有控制器注册到 DI 容器（如果尚未注册），并将IControllerActivator注册为ServiceBasedControllerActivator
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblies(appSettings.ValidatorAssemblyNames.Select(name => Assembly.Load(name))))
            .AddJsonOptions(o =>
            {
                //frombody的数据
                //不加这个 接口接收参数 string类型的时间 转换 datetime类型报错 system.text.json不支持隐式转化    //Newtonsoft.Json 等默认支持隐式转换, 不一定是个合理的方式
                //o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase //开头字母小写 默认
                o.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                o.JsonSerializerOptions.PropertyNamingPolicy = null;
                o.JsonSerializerOptions.Converters.Add(new StringJsonConvert()); //去除模型字符串首尾 空格
                o.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverter());
                o.JsonSerializerOptions.Converters.Add(new LongJsonConvert());
            }); //.AddWebApiConventions() 处理HttpResponseMessage类型返回值的问题


            //模型验证 ApiBehaviorOptions 的统一模型验证配置一定要放到(.AddMvc)后面
            services.AddValidateModelService();

            //Http
            services.AddHttpClient();

            //数据保护
            services.AddDataProtection();

            //雪花id
            services.AddSingleton(new IdWorker(appSettings.SnowflakeId.WorkerId, appSettings.SnowflakeId.DataCenterId));

            //AutoMapper
            services.AddAutoMapper(appSettings.AutoMapperAssemblyNames.Select(name => Assembly.Load(name)));

            //当前用户信息
            services.AddScoped<ICurrentUser, CurrentUser>();

            services.AutoDependency(appSettings.DiAssemblyNames.Select(name => Assembly.Load(name)));

            services.ConfigureDynamicProxy();
            
            return services;
        }
    }
}
