using AspectCore.DynamicProxy;
using Hao.Json;
using Hao.Response;
using Hao.RunTimeException;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using NLog;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Hao.Core.Extensions
{
    internal static class ExceptionMiddleware
    {
        private readonly static ILogger _logger = LogManager.GetCurrentClassLogger();

        public static void UseExceptionMiddleware(this IApplicationBuilder app)
        {
            try
            {
                app.UseExceptionHandler(new ExceptionHandlerOptions
                {
                    ExceptionHandler = Invoke
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex, H_JsonSerializer.Serialize(ex.Message));
            }
        }

        //静态方法效率上要比实例化高，静态方法的缺点是不自动进行销毁，而实例化的则可以做销毁。

        //静态方法和静态变量创建后始终使用同一块内存，而使用实例的方式会创建多个内存。
        private static async Task Invoke(HttpContext context)
        {
            context.Response.StatusCode = StatusCodes.Status200OK;
            context.Response.ContentType = "application/json";
            var response = new H_Response
            {
                Success = false,
                ErrorMsg = "系统异常"
            };

            var ex = context.Features.Get<IExceptionHandlerFeature>().Error;

            if (ex is H_Exception exception)
            {
                response.ErrorCode = exception.Code;
                response.ErrorMsg = exception.Message;
            }
            else if(ex is AspectInvocationException aspectException) 
            {
                var errorSplit = "--->";

                if (aspectException.Message.Contains(errorSplit))
                {
                    response.ErrorMsg = aspectException.Message.Split(errorSplit)[1].Trim().TrimEnd('.');
                }
            }
            else
            {
                response.ErrorMsg = ex.Message;
            }

            var errorLog = new
            {
                context.Request.Path,
                context.TraceIdentifier,
                ex.Message
            };

            _logger.Error(ex, H_JsonSerializer.Serialize(errorLog)); //异常信息，记录到日志中

            await context.Response.WriteAsync(H_JsonSerializer.Serialize(response), Encoding.UTF8);
        }
    }
}