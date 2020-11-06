using Hao.Library;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.IO;

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
        internal static IApplicationBuilder Configure(this IApplicationBuilder app, IHostEnvironment env, H_AppSettingsConfig appSettings)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"{appSettings.Swagger.Name}/swagger.json", appSettings.Swagger.Name);
                });

                app.UseCors(x => x.AllowCredentials().AllowAnyMethod().AllowAnyHeader().WithOrigins(appSettings.CorsUrls));
            }


            app.UseExceptionMiddleware();

            #region 文件
            //使用默认文件夹wwwroot
            app.UseStaticFiles();

            //文件访问权限
            app.UseWhen(a => a.Request.Path.Value.Contains(appSettings.FilePath.ExportExcelPath) || a.Request.Path.Value.Contains("file_template"), b => b.UseMiddleware<StaticFileMiddleware>());


            //导出excel路径
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

            ////头像路径
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


            // Nginx 获取ip
            app.UseForwardedHeaders(new ForwardedHeadersOptions()
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseRouting();

            //权限 .net core3.0需要放UseRouting后面
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            return app;
        }
    }
}
