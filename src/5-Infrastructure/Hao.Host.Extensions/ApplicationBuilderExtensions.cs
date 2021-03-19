using Hao.Library;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.IO;
using Serilog;
using System;

namespace Hao.Core.Extensions
{
    internal static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 用于配置中间件，以构建请求处理流水线
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="appSettings"></param>
        /// <returns></returns>
        internal static IApplicationBuilder Configure(this IApplicationBuilder app, IHostEnvironment env, H_AppSettings appSettings)
        {
            #region DevEnvironment

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"{appSettings.Swagger.Name}/swagger.json", appSettings.Swagger.Name);
                });

                app.UseCors(x => x.AllowCredentials().AllowAnyMethod().AllowAnyHeader().WithOrigins(appSettings.CorsUrls));
            }

            #endregion

            app.UseSerilogRequestLogging();

            #region Nginx

            // Nginx 获取ip
            app.UseForwardedHeaders(new ForwardedHeadersOptions()
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            #endregion


            #region 异常中间件

            // app.UseExceptionMiddleware();

            #endregion


            #region 文件信息
            //使用默认文件夹wwwroot
            app.UseStaticFiles();

            //文件访问权限
            app.UseWhen(a => a.Request.Path.Value.Contains(appSettings.FilePath.ExportExcelPath) || a.Request.Path.Value.Contains("file_template"), b => b.UseMiddleware<StaticFileMiddleware>());


            //创建文件夹，保存导出的excel文件
            if (!Directory.Exists(appSettings.FilePath.ExportExcelPath))
            {
                Directory.CreateDirectory(appSettings.FilePath.ExportExcelPath);
            }

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(appSettings.FilePath.ExportExcelPath),
                RequestPath = appSettings.RequestPath.ExportExcel,
                ContentTypeProvider = new FileExtensionContentTypeProvider(new Dictionary<string, string>
                {
                    { ".xlsx","application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"}
                })
            });

            ////创建文件夹，保存头像文件
            //if (!Directory.Exists(appSettings.FilePath.AvatarPath))
            //{
            //    Directory.CreateDirectory(appSettings.FilePath.AvatarPath);
            //}

            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(appSettings.FilePath.AvatarPath),
            //    RequestPath = appSettings.RequestPath.AvatarFile
            //});
            #endregion


            #region 路由&权限

            app.UseRouting();

            //权限 .net core3.0需要放UseRouting后面
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            #endregion


            return app;
        }
    }
}
