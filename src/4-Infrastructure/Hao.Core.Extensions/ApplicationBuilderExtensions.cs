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
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 启动webhost
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="appSettings"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseWebHost(this IApplicationBuilder app, IHostEnvironment env, H_AppSettingsConfig appSettings)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"{appSettings.Swagger.Name}/swagger.json", appSettings.Swagger.Name);
                    //c.InjectStylesheet("/css/swagger_ui.css");
                });
            }

            app.UseCors(x => x.AllowCredentials().AllowAnyMethod().AllowAnyHeader().WithOrigins(appSettings.CorsUrls));

            app.UseExceptionMiddleware();

            #region 文件
            //文件访问权限
            app.UseWhen(a => a.Request.Path.Value.Contains(appSettings.FilePath.ExportExcelPath) || a.Request.Path.Value.Contains("file_template"), b => b.UseMiddleware<StaticFileMiddleware>());
            //使用默认文件夹wwwroot
            app.UseStaticFiles();

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

            //头像路径
            if (!Directory.Exists(appSettings.FilePath.AvatarPath))
            {
                Directory.CreateDirectory(appSettings.FilePath.AvatarPath);
            }

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(appSettings.FilePath.AvatarPath),
                RequestPath = appSettings.RequestPath.AvatarFile
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

            return app;
        }
    }
}
