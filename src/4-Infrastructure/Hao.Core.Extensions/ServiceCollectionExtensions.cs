using AutoMapper;
using CSRedis;
using DotNetCore.CAP;
using FluentValidation.AspNetCore;
using Hao.Http;
using Hao.Json;
using Hao.Library;
using Hao.Snowflake;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;

namespace Hao.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWebHost(this IServiceCollection services, IHostEnvironment env, IConfiguration cfg, AppSettingsConfig appSettings)
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

            #region 单例注入，雪花id

            var worker = new IdWorker(appSettings.SnowflakeId.WorkerId, appSettings.SnowflakeId.DataCenterId);
            services.AddSingleton(worker);

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
                    ValidAudience = appSettings.Jwt.Audience,//Audience
                    ValidIssuer = appSettings.Jwt.Issuer,//Issuer，这两项和前面签发jwt的设置一致
                    IssuerSigningKey = appSettings.Jwt.SecurityKey,//拿到SecurityKey
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

            //利用分布式缓存
            //现在,ASP.NET Core引入了IDistributedCache分布式缓存接口，它是一个相当基本的分布式缓存标准API，可以让您对它进行编程，然后无缝地插入第三方分布式缓存
            //DistributedCache将拷贝缓存的文件到Slave节点
            services.AddSingleton<IDistributedCache>(new CSRedisCache(RedisHelper.Instance));

            #endregion


            #region Orm

            services.AddPostgreSQLService(appSettings.ConnectionString.PostgreSql);

            #endregion


            #region CAP
            //Note: The injection of services needs before of `services.AddCap()`
            services.Scan(a =>
            {
                a.FromAssemblies(appSettings.EventSubscribeAssemblyNames.Select(name => Assembly.Load(name)))
                 .AddClasses()
                 .AsMatchingInterface((x, p) => typeof(ICapSubscribe).IsAssignableFrom(p.GetType())) //直接或间接实现了ICapSubscribe
                 .WithTransientLifetime();
            });

            //CAP
            services.AddCapService(new H_CapConfig()
            {
                PostgreSqlConnection = appSettings.ConnectionString.PostgreSql,
                HostName = appSettings.RabbitMQ.HostName,
                VirtualHost = appSettings.RabbitMQ.VirtualHost,
                Port = appSettings.RabbitMQ.Port,
                UserName = appSettings.RabbitMQ.UserName,
                Password = appSettings.RabbitMQ.Password
            });
            #endregion


            #region AutoMapper

            services.AddAutoMapper(appSettings.AutoMapperAssemblyNames.Select(name => Assembly.Load(name)));

            #endregion 


            //替换控制器所有者,详见有道笔记,放AddMvc前面 controller属性注入
            //services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());


            services.AddControllers(x =>
            {
                x.Filters.Add(typeof(H_ResultFilter));
            })
            .AddControllersAsServices() //controller属性注入 .net core 3.1版本
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblies(appSettings.ValidatorAssemblyNames.Select(name => Assembly.Load(name))))
            .AddJsonOptions(o =>
            {
                //不加这个 接口接收参数 string类型的时间 转换 datetime类型报错 system.text.json不支持隐式转化
                //Newtonsoft.Json 等默认支持隐式转换, 不一定是个合理的方式
                o.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                o.JsonSerializerOptions.PropertyNamingPolicy = null;
                o.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverter());
                o.JsonSerializerOptions.Converters.Add(new LongJsonConvert());
            }); //.AddWebApiConventions() 处理HttpResponseMessage类型返回值的问题


            //模型验证 ApiBehaviorOptions 的统一模型验证配置一定要放到(.AddMvc)后面
            services.AddCheckViewModelService();

            //Http
            services.AddHttpClient();

            //数据保护
            services.AddDataProtection();

            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<IHttpProvider, HttpProvider>();

            return services;
        }
    }
}
