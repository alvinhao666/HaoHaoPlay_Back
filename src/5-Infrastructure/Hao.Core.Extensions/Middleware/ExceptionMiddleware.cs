using AspectCore.DynamicProxy;
using Hao.Response;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hao.Core.Extensions
{
    /// <summary>
    /// 异常中间件
    /// </summary>
    internal static class ExceptionMiddleware
    {
        public static void UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = Invoke
            });
        }

        //静态方法效率上要比实例化高，静态方法的缺点是不自动进行销毁，而实例化的则可以做销毁。

        //静态方法和静态变量创建后始终使用同一块内存，而使用实例的方式会创建多个内存。
        /// <summary>
        /// 异常处理，不会处理eventbus中的异常
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static async Task Invoke(HttpContext context)
        {
            context.Response.StatusCode = StatusCodes.Status200OK;
            context.Response.ContentType = "application/json";
            
            var response = new H_Response {Success = false};

            var ex = context.Features.Get<IExceptionHandlerFeature>().Error;

            if (ex is H_Exception exception)
            {
                response.ErrorCode = exception.Code;
                response.ErrorMsg = exception.Message;
            }
            else if(ex is AspectInvocationException aspectException) 
            {
                response.ErrorMsg = aspectException.InnerException?.Message;
            }
#if DEBUG  //开发环境 能看具体错误
            else
            {
                response.ErrorMsg = ex.Message;
            }
#endif
            if (string.IsNullOrWhiteSpace(response.ErrorMsg)) response.ErrorMsg = "系统异常";

            H_Log.Error(ex, new LogNote()
            {
                Location = context.Request.Path.Value,
                TraceId = context.TraceIdentifier,
                Extra = "系统异常信息"
            }); //异常信息，记录到日志中

            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, //解决中文乱码
                PropertyNamingPolicy = null //PropertyNamingPolicy = JsonNamingPolicy.CamelCase //开头字母小写 默认
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options), Encoding.UTF8);
        }
    }
}